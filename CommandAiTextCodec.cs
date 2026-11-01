using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>Reads and writes the complete line-oriented CommandAi/2 proposal envelope.</summary>
/// <remarks>This codec validates representation only. It does not interpret preconditions, authorize commands, or execute them.</remarks>
public static class CommandAiTextCodec
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 16
    };

    public static string Encode(CommandProposal proposal)
    {
        ArgumentNullException.ThrowIfNull(proposal);
        var lines = new List<string> { "[CommandAi]", $"Protocol={proposal.ProtocolVersion}" };
        foreach (var command in proposal.Commands)
        {
            var wire = new CommandWire(
                command.CommandId,
                command.ActionId,
                ToWire(command.Target),
                ToWire(command.Scope),
                command.IdempotencyKey,
                new OutcomeWire(
                    command.OutcomeContract.OutcomeId,
                    command.OutcomeContract.Fields.Select(field =>
                        new OutcomeFieldWire(field.FieldId, field.ValueKind.ToString(), field.Required)).ToArray()),
                command.Arguments.Select(argument =>
                    new ArgumentWire(argument.ParameterId, ToWire(argument.Value))).ToArray(),
                command.Preconditions.Select(precondition =>
                    new PreconditionWire(precondition.GrammarVersion, precondition.Expression)).ToArray());

            lines.Add("Command=" + JsonSerializer.Serialize(wire, JsonOptions));
        }

        lines.Add("[/CommandAi]");
        var encoded = string.Join(Environment.NewLine, lines);
        if (encoded.Length > CommandLimits.MaxEnvelopeCharacters)
            throw new ArgumentException($"Encoded proposal cannot exceed {CommandLimits.MaxEnvelopeCharacters} characters.", nameof(proposal));
        return encoded;
    }

    public static CommandProposal Decode(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("CommandAi text cannot be empty.", nameof(text));
        if (text.Length > CommandLimits.MaxEnvelopeCharacters)
            throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxEnvelopeCharacters}-character limit.");

        var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim()).ToArray();
        if (lines.Length > CommandLimits.MaxCommands + 3)
            throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxCommands}-command limit.");

        var start = Array.FindIndex(lines, line => line.Equals("[CommandAi]", StringComparison.Ordinal));
        if (start != 0)
            throw new FormatException("Opening [CommandAi] marker must be the first envelope line.");
        var end = Array.FindIndex(lines, start + 1, line => line.Equals("[/CommandAi]", StringComparison.Ordinal));
        if (end <= start)
            throw new FormatException("A complete [/CommandAi] closing marker was not found.");
        if (end != lines.Length - 1)
            throw new FormatException("Unexpected content appears after the CommandAi envelope.");

        string? protocol = null;
        var commands = new List<CommandInstruction>();
        for (var i = start + 1; i < end; i++)
        {
            var line = lines[i];
            if (line.StartsWith("Protocol=", StringComparison.Ordinal))
            {
                if (protocol is not null)
                    throw new FormatException("Protocol may be declared only once.");
                protocol = line["Protocol=".Length..].Trim();
            }
            else if (line.StartsWith("Command=", StringComparison.Ordinal))
            {
                if (commands.Count >= CommandLimits.MaxCommands)
                    throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxCommands}-command limit.");
                commands.Add(ParseCommand(line["Command=".Length..].Trim()));
            }
            else
            {
                throw new FormatException($"Unrecognized envelope entry on line {i + 1}.");
            }
        }

        if (!string.Equals(protocol, CommandProposal.CurrentProtocolVersion, StringComparison.Ordinal))
            throw new FormatException($"Unsupported or missing CommandAi protocol '{protocol ?? "(missing)"}'.");

        try
        {
            return new CommandProposal(commands, protocol!);
        }
        catch (ArgumentException ex)
        {
            throw new FormatException("The CommandAi envelope does not describe a valid proposal.", ex);
        }
    }

    private static CommandInstruction ParseCommand(string json)
    {
        CommandWire wire;
        try
        {
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions { MaxDepth = 16 });
            if (document.RootElement.ValueKind != JsonValueKind.Object)
                throw new FormatException("A command entry must be a JSON object.");
            RejectDuplicateProperties(document.RootElement);

            wire = JsonSerializer.Deserialize<CommandWire>(json, JsonOptions)
                ?? throw new FormatException("A command entry cannot be JSON null.");
        }
        catch (JsonException ex)
        {
            throw new FormatException("A command entry contains invalid or unsupported JSON.", ex);
        }

        if (wire.Target is null || wire.Scope is null || wire.OutcomeContract is null ||
            wire.Arguments is null || wire.Preconditions is null || wire.IdempotencyKey is null)
            throw new FormatException("A command entry is missing required contract fields.");

        try
        {
            var outcomeFields = wire.OutcomeContract.Fields
                ?? throw new FormatException("The outcome contract must declare its fields collection.");
            var fields = outcomeFields.Select(field =>
            {
                if (field is null || field.Required is null ||
                    !Enum.TryParse<CommandValueKind>(field.ValueKind, false, out var kind) ||
                    !Enum.IsDefined(kind))
                    throw new FormatException("An outcome field is missing required data or contains an unsupported value kind.");
                return new CommandOutcomeField(field.FieldId, kind, field.Required.Value);
            }).ToArray();

            var arguments = wire.Arguments.Select(argument =>
            {
                if (argument is null || argument.Value is null)
                    throw new FormatException("A command argument is missing its value.");
                return new CommandArgument(argument.ParameterId, FromWire(argument.Value));
            }).ToArray();

            var preconditions = wire.Preconditions.Select(precondition =>
            {
                if (precondition is null)
                    throw new FormatException("A command precondition cannot be null.");
                return new CommandPrecondition(precondition.GrammarVersion, precondition.Expression);
            }).ToArray();

            return new CommandInstruction(
                wire.CommandId,
                wire.ActionId,
                FromWire(wire.Target),
                FromWire(wire.Scope),
                wire.IdempotencyKey,
                new CommandOutcomeContract(wire.OutcomeContract.OutcomeId, fields),
                arguments,
                preconditions);
        }
        catch (ArgumentException ex)
        {
            throw new FormatException("The command contains invalid or duplicate contract fields.", ex);
        }
    }

    private static void RejectDuplicateProperties(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in element.EnumerateObject())
            {
                if (!names.Add(property.Name))
                    throw new FormatException($"Duplicate JSON property '{property.Name}' is not allowed.");
                RejectDuplicateProperties(property.Value);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
                RejectDuplicateProperties(item);
        }
    }

    private static ValueWire ToWire(CommandValue value) => value.Kind switch
    {
        CommandValueKind.IntegerIdentifier => new ValueWire("IntegerIdentifier", value.Identifier, null),
        CommandValueKind.LiteralText => new ValueWire("LiteralText", null, value.Text),
        _ => throw new FormatException($"Unsupported value kind '{value.Kind}'.")
    };

    private static CommandValue FromWire(ValueWire value)
    {
        return value.Kind switch
        {
            "IntegerIdentifier" when value.Identifier is > 0 && value.Text is null =>
                CommandValue.FromIdentifier(value.Identifier.Value),
            "LiteralText" when value.Identifier is null && value.Text is not null =>
                CommandValue.FromText(value.Text),
            _ => throw new FormatException("A typed command value has an invalid kind or field combination.")
        };
    }

    private sealed record CommandWire(
        Guid CommandId,
        ulong ActionId,
        ValueWire Target,
        ValueWire Scope,
        string IdempotencyKey,
        OutcomeWire OutcomeContract,
        IReadOnlyList<ArgumentWire> Arguments,
        IReadOnlyList<PreconditionWire> Preconditions);

    private sealed record ValueWire(string Kind, ulong? Identifier, string? Text);
    private sealed record ArgumentWire(ulong ParameterId, ValueWire Value);
    private sealed record PreconditionWire(string GrammarVersion, string Expression);
    private sealed record OutcomeWire(ulong OutcomeId, IReadOnlyList<OutcomeFieldWire> Fields);
    private sealed record OutcomeFieldWire(ulong FieldId, string ValueKind, bool? Required);
}

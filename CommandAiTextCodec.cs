using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>Reads and writes compact and enriched atomic-command forms within CommandAi/1.</summary>
/// <remarks>
/// Compact commands retain the original Command= action/argument line. Enriched commands use
/// AtomicCommand= followed by a bounded JSON record. Older compact-only readers reject the new
/// AtomicCommand entry; they must be upgraded before consuming enriched proposals.
/// This codec validates representation only. It does not resolve meaning, evaluate preconditions,
/// authorize commands, guarantee idempotent delivery, or execute them.
/// </remarks>
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
            if (command.HasAtomicContract)
            {
                var wire = new AtomicCommandWire(
                    command.CommandId!.Value,
                    command.ActionId,
                    ToWire(command.Target!),
                    ToWire(command.Scope!),
                    command.IdempotencyKey!,
                    new OutcomeWire(
                        command.OutcomeContract!.OutcomeId,
                        command.OutcomeContract.Fields.Select(field =>
                            new OutcomeFieldWire(field.FieldId, field.ValueKind.ToString(), field.Required)).ToArray()),
                    command.Arguments.Select(argument =>
                        new ArgumentWire(argument.ParameterId, ToWire(argument.Value))).ToArray(),
                    command.Preconditions.Select(precondition =>
                        new PreconditionWire(precondition.GrammarVersion, precondition.Expression)).ToArray(),
                    command.GrammarBody is null ? null : new GrammarBodyWire(
                        command.GrammarBody.GrammarWireVersion, command.GrammarBody.PayloadBase64));

                lines.Add("AtomicCommand=" + JsonSerializer.Serialize(wire, JsonOptions));
                continue;
            }

            var tokens = new List<string> { command.ActionId.ToString(CultureInfo.InvariantCulture) };
            foreach (var argument in command.Arguments)
            {
                var value = argument.Value.Kind switch
                {
                    CommandValueKind.IntegerIdentifier => argument.Value.Identifier!.Value.ToString(CultureInfo.InvariantCulture),
                    CommandValueKind.LiteralText => JsonSerializer.Serialize(argument.Value.Text),
                    _ => throw new FormatException($"Unsupported value kind '{argument.Value.Kind}'.")
                };
                tokens.Add($"{argument.ParameterId.ToString(CultureInfo.InvariantCulture)}={value}");
            }
            lines.Add("Command=" + string.Join(' ', tokens));
        }

        lines.Add("[/CommandAi]");
        var encoded = string.Join("\n", lines);
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
        if (start < 0)
            throw new FormatException("Opening [CommandAi] marker was not found.");
        if (start != 0)
            throw new FormatException("Unexpected content appears before the CommandAi envelope.");

        var end = Array.FindIndex(lines, start + 1, line => line.Equals("[/CommandAi]", StringComparison.Ordinal));
        if (end <= start)
            throw new FormatException("A complete [/CommandAi] closing marker was not found.");
        if (end != lines.Length - 1)
            throw new FormatException("Unexpected content appears after the CommandAi envelope.");
        if (Array.FindIndex(lines, start + 1, line => line.Equals("[CommandAi]", StringComparison.Ordinal)) >= 0)
            throw new FormatException("Nested CommandAi envelopes are not supported.");

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
                EnsureCommandCapacity(commands);
                try
                {
                    commands.Add(ParseCompactCommand(line["Command=".Length..].Trim()));
                }
                catch (ArgumentException ex)
                {
                    throw new FormatException("A compact command contains invalid or duplicate arguments.", ex);
                }
            }
            else if (line.StartsWith("AtomicCommand=", StringComparison.Ordinal))
            {
                EnsureCommandCapacity(commands);
                commands.Add(ParseAtomicCommand(line["AtomicCommand=".Length..].Trim()));
            }
            else if (line.Length > 0)
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

    private static void EnsureCommandCapacity(IReadOnlyCollection<CommandInstruction> commands)
    {
        if (commands.Count >= CommandLimits.MaxCommands)
            throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxCommands}-command limit.");
    }

    private static CommandInstruction ParseCompactCommand(string payload)
    {
        var tokens = Tokenize(payload);
        if (tokens.Count == 0 ||
            !ulong.TryParse(tokens[0], NumberStyles.None, CultureInfo.InvariantCulture, out var actionId) ||
            actionId == 0)
            throw new FormatException("A compact command must begin with a positive integer action identifier.");
        if (tokens.Count - 1 > CommandLimits.MaxArgumentsPerCommand)
            throw new FormatException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.");

        var arguments = new List<CommandArgument>();
        foreach (var token in tokens.Skip(1))
        {
            var separator = token.IndexOf('=');
            if (separator <= 0 ||
                !ulong.TryParse(token[..separator], NumberStyles.None, CultureInfo.InvariantCulture, out var parameterId) ||
                parameterId == 0)
                throw new FormatException($"Invalid parameter token '{token}'.");

            var rawValue = token[(separator + 1)..];
            CommandValue value;
            if (rawValue.StartsWith('"'))
            {
                try
                {
                    value = CommandValue.FromText(
                        JsonSerializer.Deserialize<string>(rawValue) ??
                        throw new FormatException("Literal value cannot be null."));
                }
                catch (JsonException ex)
                {
                    throw new FormatException($"Invalid quoted literal in '{token}'.", ex);
                }
                catch (ArgumentException ex)
                {
                    throw new FormatException($"Literal value in '{token}' exceeds CommandAi limits.", ex);
                }
            }
            else if (ulong.TryParse(rawValue, NumberStyles.None, CultureInfo.InvariantCulture, out var identifier) && identifier > 0)
            {
                value = CommandValue.FromIdentifier(identifier);
            }
            else
            {
                throw new FormatException($"Value '{rawValue}' must be a positive integer identifier or a JSON-quoted string.");
            }

            arguments.Add(new CommandArgument(parameterId, value));
        }

        return new CommandInstruction(actionId, arguments);
    }

    private static CommandInstruction ParseAtomicCommand(string json)
    {
        AtomicCommandWire wire;
        try
        {
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions { MaxDepth = 16 });
            if (document.RootElement.ValueKind != JsonValueKind.Object)
                throw new FormatException("An enriched atomic command must be a JSON object.");
            if (document.RootElement.TryGetProperty("body", out var bodyProperty) &&
                bodyProperty.ValueKind != JsonValueKind.Object)
                throw new FormatException("An optional grammar body, when present, must be a JSON object.");
            EnsureNoDuplicateProperties(document.RootElement);
            wire = JsonSerializer.Deserialize<AtomicCommandWire>(json, JsonOptions)
                ?? throw new FormatException("An enriched atomic command cannot be JSON null.");
        }
        catch (JsonException ex)
        {
            throw new FormatException("An enriched atomic command contains invalid or unsupported JSON.", ex);
        }

        if (wire.Target is null || wire.Scope is null || wire.IdempotencyKey is null ||
            wire.OutcomeContract is null || wire.Arguments is null || wire.Preconditions is null)
            throw new FormatException("An enriched atomic command is missing required contract fields.");

        try
        {
            var outcomeFields = wire.OutcomeContract.Fields
                ?? throw new FormatException("The outcome contract must declare its fields collection.");
            var fields = outcomeFields.Select(field =>
            {
                if (field is null ||
                    !Enum.TryParse<CommandValueKind>(field.ValueKind, false, out var kind) ||
                    !Enum.IsDefined(kind) ||
                    field.Required is null)
                    throw new FormatException("An outcome field contains an unsupported value kind or is missing its required flag.");
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

            var grammarBody = wire.Body is null
                ? null
                : CommandGrammarBody.FromBase64(wire.Body.GrammarWireVersion, wire.Body.PayloadBase64);

            return new CommandInstruction(
                wire.CommandId,
                wire.ActionId,
                FromWire(wire.Target),
                FromWire(wire.Scope),
                wire.IdempotencyKey,
                new CommandOutcomeContract(wire.OutcomeContract.OutcomeId, fields),
                arguments,
                preconditions,
                grammarBody);
        }
        catch (ArgumentException ex)
        {
            throw new FormatException("The enriched atomic command contains invalid or duplicate contract fields.", ex);
        }
    }

    private static void EnsureNoDuplicateProperties(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in element.EnumerateObject())
            {
                if (!names.Add(property.Name))
                    throw new FormatException($"Duplicate JSON property '{property.Name}' is not supported.");
                EnsureNoDuplicateProperties(property.Value);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
                EnsureNoDuplicateProperties(item);
        }
    }

    private static ValueWire ToWire(CommandValue value) => value.Kind switch
    {
        CommandValueKind.IntegerIdentifier => new ValueWire("IntegerIdentifier", value.Identifier, null),
        CommandValueKind.LiteralText => new ValueWire("LiteralText", null, value.Text),
        _ => throw new FormatException($"Unsupported value kind '{value.Kind}'.")
    };

    private static CommandValue FromWire(ValueWire? value)
    {
        if (value is null)
            throw new FormatException("A typed command value is required.");

        return value.Kind switch
        {
            "IntegerIdentifier" when value.Identifier is > 0 && value.Text is null =>
                CommandValue.FromIdentifier(value.Identifier.Value),
            "LiteralText" when value.Identifier is null && value.Text is not null =>
                CommandValue.FromText(value.Text),
            _ => throw new FormatException("A typed command value has an invalid kind or field combination.")
        };
    }

    private static List<string> Tokenize(string payload)
    {
        var tokens = new List<string>();
        var start = -1;
        var quoted = false;
        var escaped = false;
        for (var i = 0; i < payload.Length; i++)
        {
            var c = payload[i];
            if (start < 0)
            {
                if (char.IsWhiteSpace(c))
                    continue;
                start = i;
            }
            if (escaped)
            {
                escaped = false;
                continue;
            }
            if (quoted && c == '\\')
            {
                escaped = true;
                continue;
            }
            if (c == '"')
                quoted = !quoted;
            if (char.IsWhiteSpace(c) && !quoted)
            {
                tokens.Add(payload[start..i]);
                start = -1;
            }
        }
        if (quoted)
            throw new FormatException("Unterminated quoted literal.");
        if (start >= 0)
            tokens.Add(payload[start..]);
        return tokens;
    }

    private sealed record AtomicCommandWire(
        Guid CommandId,
        ulong ActionId,
        ValueWire Target,
        ValueWire Scope,
        string IdempotencyKey,
        OutcomeWire OutcomeContract,
        IReadOnlyList<ArgumentWire> Arguments,
        IReadOnlyList<PreconditionWire> Preconditions,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] GrammarBodyWire? Body = null);

    private sealed record ValueWire(string Kind, ulong? Identifier, string? Text);
    private sealed record ArgumentWire(ulong ParameterId, ValueWire Value);
    private sealed record PreconditionWire(string GrammarVersion, string Expression);
    private sealed record GrammarBodyWire(int GrammarWireVersion, string PayloadBase64);
    private sealed record OutcomeWire(ulong OutcomeId, IReadOnlyList<OutcomeFieldWire> Fields);
    private sealed record OutcomeFieldWire(ulong FieldId, string ValueKind, bool? Required);
}

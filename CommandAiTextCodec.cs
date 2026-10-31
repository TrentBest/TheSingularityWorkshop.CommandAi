using System.Globalization;
using System.Text.Json;
namespace TheSingularityWorkshop.CommandAi;

/// <summary>Reads and writes the line-oriented CommandAi/1 proposal envelope.</summary>
/// <remarks>This codec validates representation only. It does not resolve action meaning, authorize commands, or execute them.</remarks>
public static class CommandAiTextCodec
{
    public static string Encode(CommandProposal proposal)
    {
        ArgumentNullException.ThrowIfNull(proposal);
        var lines = new List<string> { "[CommandAi]", $"Protocol={proposal.ProtocolVersion}" };
        foreach (var command in proposal.Commands)
        {
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
        var encoded = string.Join(Environment.NewLine, lines);
        if (encoded.Length > CommandLimits.MaxEnvelopeCharacters)
            throw new ArgumentException($"Encoded proposal cannot exceed {CommandLimits.MaxEnvelopeCharacters} characters.", nameof(proposal));
        return encoded;
    }

    public static CommandProposal Decode(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("CommandAi text cannot be empty.", nameof(text));
        if (text.Length > CommandLimits.MaxEnvelopeCharacters)
            throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxEnvelopeCharacters}-character limit.");
        var lines = text.Split(new[] { "\\r\\n", "\\n" }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToArray();
        if (lines.Length > CommandLimits.MaxCommands + 3)
            throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxCommands}-command limit.");
        var start = Array.FindIndex(lines, line => line.Equals("[CommandAi]", StringComparison.Ordinal));
        if (start < 0) throw new FormatException("Opening [CommandAi] marker was not found.");
        if (start != 0) throw new FormatException("Unexpected content appears before the CommandAi envelope.");
        var end = Array.FindIndex(lines, start + 1, line => line.Equals("[/CommandAi]", StringComparison.Ordinal));
        if (end <= start) throw new FormatException("A complete [/CommandAi] closing marker was not found.");
        if (end != lines.Length - 1) throw new FormatException("Unexpected content appears after the CommandAi envelope.");
        if (Array.FindIndex(lines, start + 1, line => line.Equals("[CommandAi]", StringComparison.Ordinal)) >= 0)
            throw new FormatException("Nested CommandAi envelopes are not supported.");
        string? protocol = null;
        var commands = new List<CommandInstruction>();
        for (var i = start + 1; i < end; i++)
        {
            var line = lines[i];
            if (line.StartsWith("Protocol=", StringComparison.Ordinal))
            {
                if (protocol is not null) throw new FormatException("Protocol may be declared only once.");
                protocol = line["Protocol=".Length..].Trim();
            }
            else if (line.StartsWith("Command=", StringComparison.Ordinal))
            {
                if (commands.Count >= CommandLimits.MaxCommands)
                    throw new FormatException($"CommandAi envelope exceeds the {CommandLimits.MaxCommands}-command limit.");
                commands.Add(ParseCommand(line["Command=".Length..].Trim()));
            }
            else if (line.Length > 0) throw new FormatException($"Unrecognized envelope entry on line {i + 1}.");
        }
        if (!string.Equals(protocol, CommandProposal.CurrentProtocolVersion, StringComparison.Ordinal))
            throw new FormatException($"Unsupported or missing CommandAi protocol '{protocol ?? "(missing)"}'.");
        return new CommandProposal(commands, protocol!);
    }

    private static CommandInstruction ParseCommand(string payload)
    {
        var tokens = Tokenize(payload);
        if (tokens.Count == 0 || !ulong.TryParse(tokens[0], NumberStyles.None, CultureInfo.InvariantCulture, out var actionId) || actionId <= 0)
            throw new FormatException("A command must begin with a positive integer action identifier.");
        if (tokens.Count - 1 > CommandLimits.MaxArgumentsPerCommand)
            throw new FormatException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.");
        var arguments = new List<CommandArgument>();
        foreach (var token in tokens.Skip(1))
        {
            var separator = token.IndexOf('=');
            if (separator <= 0 || !ulong.TryParse(token[..separator], NumberStyles.None, CultureInfo.InvariantCulture, out var parameterId) || parameterId <= 0)
                throw new FormatException($"Invalid parameter token '{token}'.");
            var rawValue = token[(separator + 1)..];
            CommandValue value;
            if (rawValue.StartsWith('"'))
            {
                try { value = CommandValue.FromText(JsonSerializer.Deserialize<string>(rawValue) ?? throw new FormatException("Literal value cannot be null.")); }
                catch (JsonException ex) { throw new FormatException($"Invalid quoted literal in '{token}'.", ex); }
                catch (ArgumentException ex) { throw new FormatException($"Literal value in '{token}' exceeds CommandAi limits.", ex); }
            }
            else if (ulong.TryParse(rawValue, NumberStyles.None, CultureInfo.InvariantCulture, out var identifier) && identifier > 0)
                value = CommandValue.FromIdentifier(identifier);
            else throw new FormatException($"Value '{rawValue}' must be a positive integer identifier or a JSON-quoted string.");
            arguments.Add(new CommandArgument(parameterId, value));
        }
        return new CommandInstruction(actionId, arguments);
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
            if (start < 0) { if (char.IsWhiteSpace(c)) continue; start = i; }
            if (escaped) { escaped = false; continue; }
            if (quoted && c == '\\\\') { escaped = true; continue; }
            if (c == '"') quoted = !quoted;
            if (char.IsWhiteSpace(c) && !quoted) { tokens.Add(payload[start..i]); start = -1; }
        }
        if (quoted) throw new FormatException("Unterminated quoted literal.");
        if (start >= 0) tokens.Add(payload[start..]);
        return tokens;
    }
}

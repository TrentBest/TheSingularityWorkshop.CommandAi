using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>A versioned ordered collection of complete atomic command proposals.</summary>
/// <remarks>A proposal is not a transaction. A host independently validates and authorizes every command.</remarks>
public sealed record CommandProposal
{
    /// <summary>The current breaking wire-contract version.</summary>
    public const string CurrentProtocolVersion = "CommandAi/2";

    public CommandProposal(IEnumerable<CommandInstruction> commands, string protocolVersion = CurrentProtocolVersion)
    {
        if (!string.Equals(protocolVersion, CurrentProtocolVersion, StringComparison.Ordinal))
            throw new ArgumentException($"Unsupported protocol version '{protocolVersion}'.", nameof(protocolVersion));
        ArgumentNullException.ThrowIfNull(commands);

        var items = commands.Take(CommandLimits.MaxCommands + 1).ToArray();
        if (items.Length == 0)
            throw new ArgumentException("A proposal must contain at least one command.", nameof(commands));
        if (items.Length > CommandLimits.MaxCommands)
            throw new ArgumentException($"A proposal cannot contain more than {CommandLimits.MaxCommands} commands.", nameof(commands));
        if (items.Any(item => item is null))
            throw new ArgumentException("Commands cannot contain null items.", nameof(commands));

        var totalLiteralCharacters = items.Sum(command =>
            LiteralLength(command.Target) +
            LiteralLength(command.Scope) +
            command.Arguments.Sum(argument => LiteralLength(argument.Value)) +
            command.Preconditions.Sum(precondition => (long)precondition.Expression.Length) +
            command.IdempotencyKey.Length);
        if (totalLiteralCharacters > CommandLimits.MaxTotalLiteralCharacters)
            throw new ArgumentException($"A proposal cannot contain more than {CommandLimits.MaxTotalLiteralCharacters} aggregate literal characters.", nameof(commands));

        var duplicateCommandIds = items.Select(item => item.CommandId).Distinct().Count() != items.Length;
        if (duplicateCommandIds)
            throw new ArgumentException("A proposal cannot repeat a command identity.", nameof(commands));

        var duplicateIdempotencyKeys = items.Select(item => item.IdempotencyKey).Distinct(StringComparer.Ordinal).Count() != items.Length;
        if (duplicateIdempotencyKeys)
            throw new ArgumentException("A proposal cannot repeat an idempotency key.", nameof(commands));

        ProtocolVersion = protocolVersion;
        Commands = new ReadOnlyCollection<CommandInstruction>(items);
    }

    /// <summary>Wire protocol version for this proposal.</summary>
    public string ProtocolVersion { get; }
    /// <summary>Commands in their declared order.</summary>
    public IReadOnlyList<CommandInstruction> Commands { get; }

    private static long LiteralLength(CommandValue value) =>
        value.Kind == CommandValueKind.LiteralText ? value.Text!.Length : 0;
}

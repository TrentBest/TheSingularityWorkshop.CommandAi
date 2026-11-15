using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>A versioned ordered collection of compact or enriched atomic command proposals.</summary>
/// <remarks>A proposal is not a transaction. A host independently validates and authorizes every command.</remarks>
public sealed record CommandProposal
{
    /// <summary>The current wire protocol. Enriched atomic commands are an additive CommandAi/1 extension.</summary>
    public const string CurrentProtocolVersion = "CommandAi/1";

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
            (command.IdempotencyKey?.Length ?? 0) +
            command.Preconditions.Sum(precondition => (long)precondition.GrammarVersion.Length + precondition.Expression.Length) +
            command.Arguments.Sum(argument => LiteralLength(argument.Value)));
        if (totalLiteralCharacters > CommandLimits.MaxTotalLiteralCharacters)
            throw new ArgumentException($"A proposal cannot contain more than {CommandLimits.MaxTotalLiteralCharacters} aggregate literal characters.", nameof(commands));

        var totalGrammarBodyBytes = items.Sum(command => (long)(command.GrammarBody?.PayloadLength ?? 0));
        if (totalGrammarBodyBytes > CommandLimits.MaxGrammarBodyBytesPerProposal)
            throw new ArgumentException($"A proposal cannot contain more than {CommandLimits.MaxGrammarBodyBytesPerProposal} aggregate GrammarAi body bytes.", nameof(commands));

        var enriched = items.Where(command => command.HasAtomicContract).ToArray();
        if (enriched.Select(command => command.CommandId).Distinct().Count() != enriched.Length)
            throw new ArgumentException("A proposal cannot repeat an enriched command identity.", nameof(commands));
        if (enriched.Select(command => command.IdempotencyKey).Distinct(StringComparer.Ordinal).Count() != enriched.Length)
            throw new ArgumentException("A proposal cannot repeat an enriched command idempotency identity.", nameof(commands));

        ProtocolVersion = protocolVersion;
        Commands = new ReadOnlyCollection<CommandInstruction>(items);
    }

    /// <summary>Wire protocol version for this proposal.</summary>
    public string ProtocolVersion { get; }

    /// <summary>Commands in their declared order; compact and enriched /1 instructions may coexist.</summary>
    public IReadOnlyList<CommandInstruction> Commands { get; }

    private static long LiteralLength(CommandValue? value) =>
        value?.Kind == CommandValueKind.LiteralText ? value.Text!.Length : 0;
}

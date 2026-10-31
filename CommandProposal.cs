using System.Collections.ObjectModel;
namespace TheSingularityWorkshop.CommandAi;

/// <summary>A versioned collection of proposed commands. A host must independently validate and authorize every proposal.</summary>
public sealed record CommandProposal
{
    public const string CurrentProtocolVersion = "CommandAi/1";
    public CommandProposal(IEnumerable<CommandInstruction> commands, string protocolVersion = CurrentProtocolVersion)
    {
        if (!string.Equals(protocolVersion, CurrentProtocolVersion, StringComparison.Ordinal))
            throw new ArgumentException($"Unsupported protocol version '{protocolVersion}'.", nameof(protocolVersion));
        var items = (commands ?? throw new ArgumentNullException(nameof(commands))).ToArray();
        if (items.Length == 0) throw new ArgumentException("A proposal must contain at least one command.", nameof(commands));
        if (items.Length > CommandLimits.MaxCommands)
            throw new ArgumentException($"A proposal cannot contain more than {CommandLimits.MaxCommands} commands.", nameof(commands));
        if (items.Any(item => item is null)) throw new ArgumentException("Commands cannot contain null items.", nameof(commands));
        ProtocolVersion = protocolVersion;
        Commands = new ReadOnlyCollection<CommandInstruction>(items);
    }
    public string ProtocolVersion { get; }
    public IReadOnlyList<CommandInstruction> Commands { get; }
}
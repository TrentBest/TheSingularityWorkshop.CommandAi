namespace TheSingularityWorkshop.CommandAi;

/// <summary>Fluent, identity-neutral authoring surface for a command proposal.</summary>
/// <remarks>Application-specific command names and semantic validation belong in host-owned extensions. This mutable builder is not thread-safe.</remarks>
public sealed class CommandProposalBuilder
{
    private readonly List<CommandInstruction> _commands = [];

    /// <summary>Adds one command using a scoped argument builder.</summary>
    /// <param name="actionId">Application-owned action identity.</param>
    /// <param name="configure">Optional callback that adds ordered arguments.</param>
    /// <returns>This builder.</returns>
    public CommandProposalBuilder AddCommand(ulong actionId, Action<CommandInstructionBuilder>? configure = null)
    {
        if (_commands.Count >= CommandLimits.MaxCommands)
            throw new InvalidOperationException($"A proposal cannot contain more than {CommandLimits.MaxCommands} commands.");

        var command = new CommandInstructionBuilder(actionId);
        configure?.Invoke(command);
        _commands.Add(command.Build());
        return this;
    }

    /// <summary>Creates an immutable proposal snapshot from the commands added so far.</summary>
    public CommandProposal Build() => new(_commands);
}

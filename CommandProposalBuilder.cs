namespace TheSingularityWorkshop.CommandAi;

/// <summary>Fluent, identity-neutral authoring surface for a complete command proposal.</summary>
/// <remarks>Application-specific command names and semantic validation belong in host-owned extensions. This mutable builder is not thread-safe.</remarks>
public sealed class CommandProposalBuilder
{
    private readonly List<CommandInstruction> _commands = [];

    /// <summary>Adds one complete atomic command using a scoped argument/precondition builder.</summary>
    public CommandProposalBuilder AddCommand(
        Guid commandId,
        ulong actionId,
        CommandValue target,
        CommandValue scope,
        string idempotencyKey,
        CommandOutcomeContract outcomeContract,
        Action<CommandInstructionBuilder>? configure = null)
    {
        if (_commands.Count >= CommandLimits.MaxCommands)
            throw new InvalidOperationException($"A proposal cannot contain more than {CommandLimits.MaxCommands} commands.");

        var command = new CommandInstructionBuilder(
            commandId, actionId, target, scope, idempotencyKey, outcomeContract);
        configure?.Invoke(command);
        _commands.Add(command.Build());
        return this;
    }

    /// <summary>Creates an immutable proposal snapshot from the commands added so far.</summary>
    public CommandProposal Build() => new(_commands);
}

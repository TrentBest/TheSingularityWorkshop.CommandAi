namespace TheSingularityWorkshop.CommandAi;

/// <summary>Fluent, identity-neutral authoring surface for compact and enriched command proposals.</summary>
/// <remarks>Application-specific command names and semantic validation belong in host-owned extensions. This mutable builder is not thread-safe.</remarks>
public sealed class CommandProposalBuilder
{
    private readonly List<CommandInstruction> _commands = [];

    /// <summary>Adds one compact command using the original action-plus-arguments shape.</summary>
    public CommandProposalBuilder AddCommand(ulong actionId, Action<CommandInstructionBuilder>? configure = null)
    {
        EnsureCapacity();
        var command = new CommandInstructionBuilder(actionId);
        configure?.Invoke(command);
        _commands.Add(command.Build());
        return this;
    }

    /// <summary>Adds one enriched atomic command with explicit identity, target, scope, idempotency, and outcome contract.</summary>
    public CommandProposalBuilder AddCommand(
        Guid commandId,
        ulong actionId,
        CommandValue target,
        CommandValue scope,
        string idempotencyKey,
        CommandOutcomeContract outcomeContract,
        Action<CommandInstructionBuilder>? configure = null)
    {
        EnsureCapacity();
        var command = new CommandInstructionBuilder(
            commandId, actionId, target, scope, idempotencyKey, outcomeContract);
        configure?.Invoke(command);
        _commands.Add(command.Build());
        return this;
    }

    /// <summary>Creates an immutable proposal snapshot from the commands added so far.</summary>
    public CommandProposal Build() => new(_commands);

    private void EnsureCapacity()
    {
        if (_commands.Count >= CommandLimits.MaxCommands)
            throw new InvalidOperationException($"A proposal cannot contain more than {CommandLimits.MaxCommands} commands.");
    }
}

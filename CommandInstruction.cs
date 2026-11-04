using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>
/// One immutable command proposal. Compact legacy commands carry an action and ordered arguments;
/// enriched atomic commands additionally carry identity, target, scope, preconditions, idempotency,
/// and an expected-outcome contract.
/// </summary>
/// <remarks>
/// All values are opaque to CommandAi. This type describes intent; it does not interpret conditions,
/// grant authority, guarantee idempotent delivery, or execute the command.
/// </remarks>
public sealed record CommandInstruction
{
    /// <summary>Creates a compact CommandAi/1 instruction using the original action-plus-arguments shape.</summary>
    public CommandInstruction(ulong actionId, IEnumerable<CommandArgument>? arguments = null)
    {
        ValidateActionId(actionId);
        ActionId = actionId;
        Arguments = SnapshotArguments(arguments);
        Preconditions = Array.Empty<CommandPrecondition>();
    }

    /// <summary>Creates a fully specified generic atomic command within CommandAi/1.</summary>
    public CommandInstruction(
        Guid commandId,
        ulong actionId,
        CommandValue target,
        CommandValue scope,
        string idempotencyKey,
        CommandOutcomeContract outcomeContract,
        IEnumerable<CommandArgument>? arguments = null,
        IEnumerable<CommandPrecondition>? preconditions = null)
    {
        if (commandId == Guid.Empty)
            throw new ArgumentException("A command identity is required.", nameof(commandId));
        ValidateActionId(actionId);

        CommandId = commandId;
        ActionId = actionId;
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("An idempotency identity is required.", nameof(idempotencyKey));
        if (idempotencyKey.Length > CommandLimits.MaxIdempotencyKeyCharacters)
            throw new ArgumentException($"An idempotency identity cannot exceed {CommandLimits.MaxIdempotencyKeyCharacters} characters.", nameof(idempotencyKey));
        IdempotencyKey = idempotencyKey;
        OutcomeContract = outcomeContract ?? throw new ArgumentNullException(nameof(outcomeContract));
        Arguments = SnapshotArguments(arguments);
        Preconditions = SnapshotPreconditions(preconditions);
    }

    /// <summary>Whether this instruction carries the complete generic atomic-command contract.</summary>
    public bool HasAtomicContract => CommandId.HasValue;

    /// <summary>Unique identity of this proposed command, distinct from its idempotency identity.</summary>
    public Guid? CommandId { get; }

    /// <summary>Application-owned action identity.</summary>
    public ulong ActionId { get; }

    /// <summary>Opaque typed identity of the target; absent only on legacy compact instructions.</summary>
    public CommandValue? Target { get; }

    /// <summary>Opaque typed identity of the scope; absent only on legacy compact instructions.</summary>
    public CommandValue? Scope { get; }

    /// <summary>Caller-supplied identity for host-enforced idempotent handling; not an execution guarantee.</summary>
    public string? IdempotencyKey { get; }

    /// <summary>Ordered typed parameters.</summary>
    public IReadOnlyList<CommandArgument> Arguments { get; }

    /// <summary>Opaque grammar-versioned conditions; evaluation belongs to GrammarAi and the host.</summary>
    public IReadOnlyList<CommandPrecondition> Preconditions { get; }

    /// <summary>Expected outcome shape, not an actual execution outcome.</summary>
    public CommandOutcomeContract? OutcomeContract { get; }

    private static void ValidateActionId(ulong actionId)
    {
        if (actionId == 0)
            throw new ArgumentOutOfRangeException(nameof(actionId), "Action identifiers must be positive.");
    }

    private static IReadOnlyList<CommandArgument> SnapshotArguments(IEnumerable<CommandArgument>? arguments)
    {
        var items = (arguments ?? []).Take(CommandLimits.MaxArgumentsPerCommand + 1).ToArray();
        if (items.Length > CommandLimits.MaxArgumentsPerCommand)
            throw new ArgumentException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.", nameof(arguments));
        if (items.Any(item => item is null))
            throw new ArgumentException("Arguments cannot contain null items.", nameof(arguments));
        if (items.Select(item => item.ParameterId).Distinct().Count() != items.Length)
            throw new ArgumentException("A command cannot repeat a parameter identifier.", nameof(arguments));
        return new ReadOnlyCollection<CommandArgument>(items);
    }

    private static IReadOnlyList<CommandPrecondition> SnapshotPreconditions(IEnumerable<CommandPrecondition>? preconditions)
    {
        var items = (preconditions ?? []).Take(CommandLimits.MaxPreconditionsPerCommand + 1).ToArray();
        if (items.Length > CommandLimits.MaxPreconditionsPerCommand)
            throw new ArgumentException($"A command cannot contain more than {CommandLimits.MaxPreconditionsPerCommand} preconditions.", nameof(preconditions));
        if (items.Any(item => item is null))
            throw new ArgumentException("Preconditions cannot contain null items.", nameof(preconditions));
        return new ReadOnlyCollection<CommandPrecondition>(items);
    }
}

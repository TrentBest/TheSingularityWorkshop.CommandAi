using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>
/// One immutable atomic command proposal with identity, target, scope, parameters,
/// preconditions, idempotency identity, and an expected outcome contract.
/// </summary>
/// <remarks>
/// Values remain opaque to CommandAi. This type describes intent; it does not interpret
/// preconditions, grant authority, or execute the command.
/// </remarks>
public sealed record CommandInstruction
{
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
        if (actionId == 0)
            throw new ArgumentOutOfRangeException(nameof(actionId), "Action identifiers must be positive.");

        CommandId = commandId;
        ActionId = actionId;
        Target = target ?? throw new ArgumentNullException(nameof(target));
        Scope = scope ?? throw new ArgumentNullException(nameof(scope));

        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new ArgumentException("An idempotency key is required.", nameof(idempotencyKey));
        if (idempotencyKey.Length > CommandLimits.MaxIdempotencyKeyCharacters)
            throw new ArgumentException($"An idempotency key cannot exceed {CommandLimits.MaxIdempotencyKeyCharacters} characters.", nameof(idempotencyKey));
        IdempotencyKey = idempotencyKey;
        OutcomeContract = outcomeContract ?? throw new ArgumentNullException(nameof(outcomeContract));

        var argumentItems = (arguments ?? []).Take(CommandLimits.MaxArgumentsPerCommand + 1).ToArray();
        if (argumentItems.Length > CommandLimits.MaxArgumentsPerCommand)
            throw new ArgumentException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.", nameof(arguments));
        if (argumentItems.Any(item => item is null))
            throw new ArgumentException("Arguments cannot contain null items.", nameof(arguments));
        if (argumentItems.Select(item => item.ParameterId).Distinct().Count() != argumentItems.Length)
            throw new ArgumentException("A command cannot repeat a parameter identifier.", nameof(arguments));
        Arguments = new ReadOnlyCollection<CommandArgument>(argumentItems);

        var preconditionItems = (preconditions ?? []).Take(CommandLimits.MaxPreconditionsPerCommand + 1).ToArray();
        if (preconditionItems.Length > CommandLimits.MaxPreconditionsPerCommand)
            throw new ArgumentException($"A command cannot contain more than {CommandLimits.MaxPreconditionsPerCommand} preconditions.", nameof(preconditions));
        if (preconditionItems.Any(item => item is null))
            throw new ArgumentException("Preconditions cannot contain null items.", nameof(preconditions));
        Preconditions = new ReadOnlyCollection<CommandPrecondition>(preconditionItems);
    }

    /// <summary>Unique identity of this proposed command; distinct from its idempotency key.</summary>
    public Guid CommandId { get; }
    /// <summary>Application-owned action identity.</summary>
    public ulong ActionId { get; }
    /// <summary>Opaque typed identity of the command target.</summary>
    public CommandValue Target { get; }
    /// <summary>Opaque typed identity of the command scope.</summary>
    public CommandValue Scope { get; }
    /// <summary>Caller-supplied identity used by the host to enforce idempotent handling.</summary>
    public string IdempotencyKey { get; }
    /// <summary>Ordered command parameters.</summary>
    public IReadOnlyList<CommandArgument> Arguments { get; }
    /// <summary>Opaque grammar-backed conditions; evaluation belongs to GrammarAi and the host.</summary>
    public IReadOnlyList<CommandPrecondition> Preconditions { get; }
    /// <summary>Declaration of the expected outcome shape, not an actual execution outcome.</summary>
    public CommandOutcomeContract OutcomeContract { get; }
}

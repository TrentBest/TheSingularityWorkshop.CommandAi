namespace TheSingularityWorkshop.CommandAi;

/// <summary>Builds one complete atomic command from opaque identities and typed values.</summary>
/// <remarks>This mutable builder is not thread-safe and performs no semantic or authorization checks.</remarks>
public sealed class CommandInstructionBuilder
{
    private readonly Guid _commandId;
    private readonly ulong _actionId;
    private readonly CommandValue _target;
    private readonly CommandValue _scope;
    private readonly string _idempotencyKey;
    private readonly CommandOutcomeContract _outcomeContract;
    private readonly List<CommandArgument> _arguments = [];
    private readonly List<CommandPrecondition> _preconditions = [];

    public CommandInstructionBuilder(
        Guid commandId,
        ulong actionId,
        CommandValue target,
        CommandValue scope,
        string idempotencyKey,
        CommandOutcomeContract outcomeContract)
    {
        if (commandId == Guid.Empty) throw new ArgumentException("A command identity is required.", nameof(commandId));
        if (actionId == 0) throw new ArgumentOutOfRangeException(nameof(actionId));
        _commandId = commandId;
        _actionId = actionId;
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        _idempotencyKey = idempotencyKey;
        _outcomeContract = outcomeContract ?? throw new ArgumentNullException(nameof(outcomeContract));
    }

    /// <summary>Adds an opaque integer identifier value for a parameter.</summary>
    public CommandInstructionBuilder WithIdentifier(ulong parameterId, ulong valueId)
        => WithValue(parameterId, CommandValue.FromIdentifier(valueId));

    /// <summary>Adds literal text for a parameter.</summary>
    public CommandInstructionBuilder WithText(ulong parameterId, string value)
        => WithValue(parameterId, CommandValue.FromText(value));

    /// <summary>Adds a typed value for a parameter.</summary>
    public CommandInstructionBuilder WithValue(ulong parameterId, CommandValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (_arguments.Count >= CommandLimits.MaxArgumentsPerCommand)
            throw new InvalidOperationException($"A command cannot contain more than {CommandLimits.MaxArgumentsPerCommand} arguments.");
        if (_arguments.Any(argument => argument.ParameterId == parameterId))
            throw new ArgumentException($"Parameter identifier '{parameterId}' is already present.", nameof(parameterId));
        _arguments.Add(new CommandArgument(parameterId, value));
        return this;
    }

    /// <summary>Adds an opaque precondition expression associated with a GrammarAi version.</summary>
    public CommandInstructionBuilder WithPrecondition(string grammarVersion, string expression)
    {
        if (_preconditions.Count >= CommandLimits.MaxPreconditionsPerCommand)
            throw new InvalidOperationException($"A command cannot contain more than {CommandLimits.MaxPreconditionsPerCommand} preconditions.");
        _preconditions.Add(new CommandPrecondition(grammarVersion, expression));
        return this;
    }

    /// <summary>Creates an immutable command snapshot.</summary>
    public CommandInstruction Build() => new(
        _commandId, _actionId, _target, _scope, _idempotencyKey,
        _outcomeContract, _arguments, _preconditions);
}

namespace TheSingularityWorkshop.CommandAi;

/// <summary>Builds one generic command instruction from opaque action and parameter identities.</summary>
/// <remarks>This builder does not resolve identifiers or attach application-specific meaning to them.</remarks>
public sealed class CommandInstructionBuilder
{
    private readonly ulong _actionId;
    private readonly List<CommandArgument> _arguments = [];

    /// <summary>Creates a command builder.</summary>
    /// <param name="actionId">Application-owned action identity; must be positive.</param>
    public CommandInstructionBuilder(ulong actionId)
    {
        if (actionId == 0) throw new ArgumentOutOfRangeException(nameof(actionId));
        _actionId = actionId;
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

    /// <summary>Creates an immutable command snapshot.</summary>
    public CommandInstruction Build() => new(_actionId, _arguments);
}

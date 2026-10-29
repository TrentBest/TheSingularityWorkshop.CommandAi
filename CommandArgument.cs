namespace TheSingularityWorkshop.CommandAi;

/// <summary>A command parameter paired with an integer value identity or literal text.</summary>
public sealed record CommandArgument
{
    public CommandArgument(ulong parameterId, CommandValue value)
    {
        if (parameterId <= 0) throw new ArgumentOutOfRangeException(nameof(parameterId), "Parameter identifiers must be positive.");
        ParameterId = parameterId;
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
    public ulong ParameterId { get; }
    public CommandValue Value { get; }
}
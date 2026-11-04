using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CommandAi;

/// <summary>Describes the shape of an expected outcome for a proposed command.</summary>
/// <remarks>
/// This is a declaration, not an actual outcome, success signal, CI verdict, or proof of execution.
/// Actual correlated outcomes belong to ProtocolAi and are interpreted by the host.
/// </remarks>
public sealed record CommandOutcomeContract
{
    public CommandOutcomeContract(ulong outcomeId, IEnumerable<CommandOutcomeField>? fields = null)
    {
        if (outcomeId == 0)
            throw new ArgumentOutOfRangeException(nameof(outcomeId), "Outcome identifiers must be positive.");

        var items = (fields ?? []).Take(CommandLimits.MaxOutcomeFields + 1).ToArray();
        if (items.Length > CommandLimits.MaxOutcomeFields)
            throw new ArgumentException($"An outcome contract cannot contain more than {CommandLimits.MaxOutcomeFields} fields.", nameof(fields));
        if (items.Any(item => item is null))
            throw new ArgumentException("Outcome fields cannot contain null items.", nameof(fields));
        if (items.Select(item => item.FieldId).Distinct().Count() != items.Length)
            throw new ArgumentException("An outcome contract cannot repeat a field identifier.", nameof(fields));

        OutcomeId = outcomeId;
        Fields = new ReadOnlyCollection<CommandOutcomeField>(items);
    }

    /// <summary>Application-owned identity for the expected outcome shape.</summary>
    public ulong OutcomeId { get; }

    /// <summary>Fields expected in an outcome conforming to this contract.</summary>
    public IReadOnlyList<CommandOutcomeField> Fields { get; }
}

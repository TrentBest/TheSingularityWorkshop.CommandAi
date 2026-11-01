namespace TheSingularityWorkshop.CommandAi;

/// <summary>Declares one field in the expected outcome contract.</summary>
/// <remarks>The field identity and value kind are opaque declarations. They are not an execution result.</remarks>
public sealed record CommandOutcomeField
{
    public CommandOutcomeField(ulong fieldId, CommandValueKind valueKind, bool required = true)
    {
        if (fieldId == 0)
            throw new ArgumentOutOfRangeException(nameof(fieldId), "Outcome field identifiers must be positive.");
        if (!Enum.IsDefined(valueKind))
            throw new ArgumentOutOfRangeException(nameof(valueKind), "The outcome field value kind is not supported.");

        FieldId = fieldId;
        ValueKind = valueKind;
        Required = required;
    }

    /// <summary>Application-owned outcome field identity.</summary>
    public ulong FieldId { get; }

    /// <summary>Expected representation kind for the field value.</summary>
    public CommandValueKind ValueKind { get; }

    /// <summary>Whether the field must be present in a conforming outcome.</summary>
    public bool Required { get; }
}

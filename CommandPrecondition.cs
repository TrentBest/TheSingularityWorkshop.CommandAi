namespace TheSingularityWorkshop.CommandAi;

/// <summary>An opaque precondition expression attached to a proposed atomic command.</summary>
/// <remarks>
/// CommandAi preserves the grammar identity/version token and expression without parsing or evaluating them.
/// Consumers should pin a fully qualified definition identity and schema version as
/// DefinitionId@SchemaVersion (for example, TheSingularityWorkshop.GrammarAi.Observer@1).
/// The consumer must resolve that exact identity/version without aliases or schema substitution.
/// GrammarAi owns the expression grammar; the host owns evaluation against live state.
/// </remarks>
public sealed record CommandPrecondition
{
    public CommandPrecondition(string grammarVersion, string expression)
    {
        if (string.IsNullOrWhiteSpace(grammarVersion))
            throw new ArgumentException("A grammar version is required.", nameof(grammarVersion));
        if (grammarVersion.Length > CommandLimits.MaxGrammarVersionCharacters)
            throw new ArgumentException($"Grammar version cannot exceed {CommandLimits.MaxGrammarVersionCharacters} characters.", nameof(grammarVersion));
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("A precondition expression is required.", nameof(expression));
        if (expression.Length > CommandLimits.MaxPreconditionCharacters)
            throw new ArgumentException($"A precondition expression cannot exceed {CommandLimits.MaxPreconditionCharacters} characters.", nameof(expression));

        GrammarVersion = grammarVersion;
        Expression = expression;
    }

    /// <summary>Exact grammar definition identity and schema version, conventionally DefinitionId@SchemaVersion.</summary>
    public string GrammarVersion { get; }

    /// <summary>Opaque grammar expression; never evaluated by CommandAi.</summary>
    public string Expression { get; }
}

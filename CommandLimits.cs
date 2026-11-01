namespace TheSingularityWorkshop.CommandAi;

/// <summary>Resource limits for untrusted CommandAi proposals.</summary>
/// <remarks>These limits bound representation parsing; they do not authorize or execute commands.</remarks>
public static class CommandLimits
{
    /// <summary>Maximum encoded proposal size, measured in UTF-16 characters.</summary>
    public const int MaxEnvelopeCharacters = 262_144;
    /// <summary>Maximum number of atomic commands in one proposal.</summary>
    public const int MaxCommands = 256;
    /// <summary>Maximum number of arguments on one atomic command.</summary>
    public const int MaxArgumentsPerCommand = 64;
    /// <summary>Maximum number of preconditions on one atomic command.</summary>
    public const int MaxPreconditionsPerCommand = 32;
    /// <summary>Maximum number of declared outcome fields.</summary>
    public const int MaxOutcomeFields = 64;
    /// <summary>Maximum aggregate literal characters in one proposal.</summary>
    public const int MaxTotalLiteralCharacters = 1_048_576;
    /// <summary>Maximum length of one literal text value, measured in UTF-16 characters.</summary>
    public const int MaxLiteralCharacters = 16_384;
    /// <summary>Maximum grammar-version identifier length.</summary>
    public const int MaxGrammarVersionCharacters = 128;
    /// <summary>Maximum opaque precondition expression length.</summary>
    public const int MaxPreconditionCharacters = 16_384;
    /// <summary>Maximum idempotency-key length.</summary>
    public const int MaxIdempotencyKeyCharacters = 256;
}

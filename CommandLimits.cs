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

    /// <summary>Maximum length of one literal text argument, measured in UTF-16 characters.</summary>
    public const int MaxLiteralCharacters = 16_384;
}

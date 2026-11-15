using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.CommandAi.MicroBundle;

/// <summary>
/// Exposes the provider-neutral CommandAi proposal codec as a code-defined MicroBundle.
/// </summary>
/// <remarks>
/// This capability represents and transports proposed intent only. It does not resolve
/// application identifiers, authorize proposals, dispatch actions, or execute commands.
/// </remarks>
public sealed class CommandAiMicroBundle : IMicroBundle
{
    /// <summary>
    /// Stable Workshop capability identity. The ASCII bytes spell "COMMANDA".
    /// This identity is independent of the NuGet package and CommandAi wire versions.
    /// </summary>
    public const ulong StableId = 0x434F4D4D414E4441UL;

    /// <summary>Version of this MicroBundle capability contract.</summary>
    public const string CapabilityVersion = "1.0.0";

    /// <summary>Provider identity for the CommandAi proposal codec.</summary>
    public const string ProposalProviderId = "commandai.proposal";

    private static readonly IReadOnlyList<MicroBundleDependencyRequest> NoDependencies =
        Array.Empty<MicroBundleDependencyRequest>();

    /// <summary>Creates the stateless CommandAi proposal capability.</summary>
    public CommandAiMicroBundle()
    {
        Descriptor = new MicroBundleDescriptor(
            StableId,
            CapabilityVersion,
            providers: [new MicroBundleProvider(ProposalProviderId)]);
    }

    /// <inheritdoc />
    public MicroBundleDescriptor Descriptor { get; }

    /// <inheritdoc />
    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => NoDependencies;

    /// <summary>Encodes a proposal using the canonical CommandAi/1 text codec.</summary>
    public string Encode(CommandProposal proposal) => CommandAiTextCodec.Encode(proposal);

    /// <summary>Decodes and structurally validates a CommandAi/1 proposal.</summary>
    public CommandProposal Decode(string text) => CommandAiTextCodec.Decode(text);

    /// <inheritdoc />
    public void Load(IMicroBundleLoadContext context)
    {
        // CommandAi is a stateless representation capability. The host supplies the
        // lifecycle context, but no host state or command execution is installed here.
        ArgumentNullException.ThrowIfNull(context);
    }

    /// <inheritdoc />
    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
    {
        ArgumentNullException.ThrowIfNull(context);

        // The proposal codec has no cross-bundle composition mutations to perform.
        return false;
    }
}
using TheSingularityWorkshop.CommandAi.MicroBundle;
using TheSingularityWorkshop.MicroBundleDomain;

namespace CommandAi.Tests;

public sealed class CommandAiMicroBundleTests
{
    [Fact]
    public void Descriptor_DeclaresStableIdentityAndProposalProvider()
    {
        var bundle = new CommandAiMicroBundle();

        Assert.Equal(CommandAiMicroBundle.StableId, bundle.Descriptor.Id);
        Assert.Equal(CommandAiMicroBundle.CapabilityVersion, bundle.Descriptor.Version);
        Assert.Contains(bundle.Descriptor.Providers, provider =>
            provider.Id == CommandAiMicroBundle.ProposalProviderId);
        Assert.Empty(bundle.Dependencies);
    }

    [Fact]
    public void Codec_RoundTripsCommandAiProposal()
    {
        var bundle = new CommandAiMicroBundle();
        var proposal = new CommandProposal(
        [
            new CommandInstruction(1001,
            [
                new CommandArgument(2001, CommandValue.FromIdentifier(42))
            ])
        ]);

        var decoded = bundle.Decode(bundle.Encode(proposal));

        Assert.Equal(proposal.ProtocolVersion, decoded.ProtocolVersion);
        Assert.Single(decoded.Commands);
        Assert.Equal(proposal.Commands[0].ActionId, decoded.Commands[0].ActionId);
        Assert.Equal(proposal.Commands[0].Arguments[0].Value.Identifier,
            decoded.Commands[0].Arguments[0].Value.Identifier);
    }

    [Fact]
    public void Load_RequiresHostContext()
    {
        var bundle = new CommandAiMicroBundle();

        Assert.Throws<ArgumentNullException>(() => bundle.Load(null!));
    }

    [Fact]
    public void Load_AcceptsHostContextWithoutExecutingCommands()
    {
        var bundle = new CommandAiMicroBundle();

        bundle.Load(new LoadContext());

        Assert.Equal(CommandAiMicroBundle.StableId, bundle.Descriptor.Id);
    }

    [Fact]
    public void Arbitrate_IsStableAndReportsNoCompositionMutation()
    {
        var bundle = new CommandAiMicroBundle();

        Assert.False(bundle.Arbitrate(new ArbitrationContext(), 0));
    }

    private sealed class LoadContext : IMicroBundleLoadContext
    {
        public ulong RuntimeId => 1;

        public bool TryGetConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration)
        {
            configuration = default;
            return false;
        }
    }

    private sealed class ArbitrationContext : IMicroBundleArbitrationContext
    {
        public ulong RuntimeId => 1;
        public IReadOnlyList<IMicroBundle> Bundles => Array.Empty<IMicroBundle>();
        public object? ExperienceContext => null;
    }
}
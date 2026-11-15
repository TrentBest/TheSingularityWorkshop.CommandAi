using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandGrammarBodyTests
{
    private static CommandInstruction Enriched(byte[]? payload = null, int wireVersion = 1)
    {
        var builder = new CommandInstructionBuilder(
            Guid.NewGuid(),
            100,
            CommandValue.FromIdentifier(200),
            CommandValue.FromIdentifier(201),
            Guid.NewGuid().ToString("N"),
            new CommandOutcomeContract(300, [new CommandOutcomeField(301, CommandValueKind.IntegerIdentifier)]));
        if (payload is not null)
            builder.WithGrammarBody(new CommandGrammarBody(wireVersion, payload));
        return builder.Build();
    }

    private static string Encode(CommandInstruction command) =>
        CommandAiTextCodec.Encode(new CommandProposal([command]));

    [Fact]
    public void EnrichedCommand_WithoutBody_RoundTripsUnchanged()
    {
        var encoded = Encode(Enriched());

        Assert.DoesNotContain("\"body\"", encoded);
        var decoded = Assert.Single(CommandAiTextCodec.Decode(encoded).Commands);
        Assert.Null(decoded.GrammarBody);
        Assert.True(decoded.HasAtomicContract);
    }

    [Fact]
    public void EnrichedCommand_WithBody_RoundTripsExactBytesAndWireVersion()
    {
        byte[] payload = [0x00, 0x01, 0x7f, 0x80, 0xff];
        var encoded = Encode(Enriched(payload, wireVersion: 987));

        Assert.Contains("\"body\":{\"grammarWireVersion\":987,\"payloadBase64\":\"AAF/gP8=\"}", encoded);
        var decoded = Assert.Single(CommandAiTextCodec.Decode(encoded).Commands);
        Assert.Equal(987, decoded.GrammarBody!.GrammarWireVersion);
        Assert.Equal(payload, decoded.GrammarBody.Payload.ToArray());
    }

    [Fact]
    public void EnrichedCommand_AcceptsUnknownPositiveGrammarWireVersionOpaquely()
    {
        var decoded = Assert.Single(CommandAiTextCodec.Decode(Encode(Enriched([1, 2, 3], int.MaxValue))).Commands);

        Assert.Equal(int.MaxValue, decoded.GrammarBody!.GrammarWireVersion);
        Assert.Equal(new byte[] { 1, 2, 3 }, decoded.GrammarBody.Payload.ToArray());
    }

    [Fact]
    public void MixedProposal_PreservesCompactAndEnrichedOrderWithBody()
    {
        var proposal = new CommandProposal([
            new CommandInstruction(10),
            Enriched([4, 5, 6]),
            new CommandInstruction(11)
        ]);

        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(proposal));

        Assert.Equal(new ulong[] { 10, 100, 11 }, decoded.Commands.Select(command => command.ActionId));
        Assert.Null(decoded.Commands[0].GrammarBody);
        Assert.Equal(new byte[] { 4, 5, 6 }, decoded.Commands[1].GrammarBody!.Payload.ToArray());
        Assert.Null(decoded.Commands[2].GrammarBody);
    }

    [Fact]
    public void GrammarBody_AcceptsExactPerCommandByteLimit()
    {
        var payload = new byte[CommandLimits.MaxGrammarBodyBytesPerCommand];

        var body = new CommandGrammarBody(1, payload);

        Assert.Equal(CommandLimits.MaxGrammarBodyBytesPerCommand, body.Payload.Length);
    }

    [Fact]
    public void GrammarBody_PayloadIsDefensivelyCopiedOnInputAndRead()
    {
        byte[] source = [1, 2, 3];
        var body = new CommandGrammarBody(1, source);
        source[0] = 99;

        Assert.True(System.Runtime.InteropServices.MemoryMarshal.TryGetArray(body.Payload, out var exposed));
        exposed.Array![exposed.Offset] = 88;

        Assert.Equal(new byte[] { 1, 2, 3 }, body.Payload.ToArray());
    }

    [Fact]
    public void GrammarBody_RejectsPayloadAbovePerCommandByteLimit()
    {
        var payload = new byte[CommandLimits.MaxGrammarBodyBytesPerCommand + 1];

        Assert.Throws<ArgumentException>(() => new CommandGrammarBody(1, payload));
    }

    [Fact]
    public void GrammarBody_RejectsZeroWireVersionAndEmptyPayload()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandGrammarBody(0, [1]));
        Assert.Throws<ArgumentException>(() => new CommandGrammarBody(1, Array.Empty<byte>()));
    }

    [Fact]
    public void Proposal_AcceptsExactAggregateGrammarBodyByteLimit()
    {
        var commands = Enumerable.Range(0, CommandLimits.MaxGrammarBodyBytesPerProposal / CommandLimits.MaxGrammarBodyBytesPerCommand)
            .Select(_ => Enriched(new byte[CommandLimits.MaxGrammarBodyBytesPerCommand]))
            .ToArray();

        var proposal = new CommandProposal(commands);

        Assert.Equal(CommandLimits.MaxGrammarBodyBytesPerProposal,
            proposal.Commands.Sum(command => command.GrammarBody!.Payload.Length));
    }

    [Fact]
    public void Proposal_RejectsAggregateGrammarBodyByteLimitPlusOne()
    {
        var commands = Enumerable.Range(0, CommandLimits.MaxGrammarBodyBytesPerProposal / CommandLimits.MaxGrammarBodyBytesPerCommand)
            .Select(_ => Enriched(new byte[CommandLimits.MaxGrammarBodyBytesPerCommand]))
            .Append(Enriched([1]))
            .ToArray();

        Assert.Throws<ArgumentException>(() => new CommandProposal(commands));
    }

    [Theory]
    [InlineData("%%%")]
    [InlineData("AQI")]
    [InlineData("AQID ")]
    [InlineData("")]
    public void Decode_RejectsMalformedEmptyOrNonCanonicalBase64(string payload)
    {
        var encoded = Encode(Enriched([1, 2, 3])).Replace("AQID", payload, StringComparison.Ordinal);

        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(encoded));
    }

    [Fact]
    public void Decode_RejectsZeroGrammarWireVersion()
    {
        var encoded = Encode(Enriched([1, 2, 3])).Replace("\"grammarWireVersion\":1", "\"grammarWireVersion\":0", StringComparison.Ordinal);

        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(encoded));
    }

    [Fact]
    public void Decode_RejectsNullBodyRatherThanTreatingItAsAbsent()
    {
        var encoded = Encode(Enriched([1, 2, 3])).Replace(
            "\"body\":{\"grammarWireVersion\":1,\"payloadBase64\":\"AQID\"}",
            "\"body\":null",
            StringComparison.Ordinal);

        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(encoded));
    }

    [Fact]
    public void Encode_EnforcesEnvelopeLimitIndependentlyOfBodyAndLiteralLimits()
    {
        var builder = new CommandInstructionBuilder(
            Guid.NewGuid(), 100, CommandValue.FromIdentifier(200), CommandValue.FromIdentifier(201),
            Guid.NewGuid().ToString("N"),
            new CommandOutcomeContract(300));
        for (ulong parameterId = 1; parameterId <= 14; parameterId++)
            builder.WithText(parameterId, new string('x', CommandLimits.MaxLiteralCharacters));
        builder.WithGrammarBody(new CommandGrammarBody(1, new byte[CommandLimits.MaxGrammarBodyBytesPerCommand]));

        var proposal = new CommandProposal([builder.Build()]);

        Assert.Throws<ArgumentException>(() => CommandAiTextCodec.Encode(proposal));
    }
}

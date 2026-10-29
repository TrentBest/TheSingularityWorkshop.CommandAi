using TheSingularityWorkshop.CommandAi;
namespace CommandAi.Tests;
public sealed class CommandAiTextCodecTests
{
    [Fact]
    public void EncodeThenDecode_PreservesIntegerAndLiteralArguments()
    {
        var original = new CommandProposal([new CommandInstruction(100, [
            new CommandArgument(200, CommandValue.FromIdentifier(302)),
            new CommandArgument(201, CommandValue.FromText("Logs.Diagnostic Stream"))
        ])]);
        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(original));
        Assert.Equal(original.ProtocolVersion, decoded.ProtocolVersion);
        Assert.Single(decoded.Commands);
        Assert.Equal(100UL, decoded.Commands[0].ActionId);
        Assert.Equal(2, decoded.Commands[0].Arguments.Count);
        Assert.Equal(302UL, decoded.Commands[0].Arguments[0].Value.Identifier);
        Assert.Equal("Logs.Diagnostic Stream", decoded.Commands[0].Arguments[1].Value.Text);
    }

    [Fact]
    public void EncodeThenDecode_PreservesQuotedTextAndBackslashes()
    {
        var quoted = """a "quoted" value with spaces""";
        var path = @"path\with\slashes";
        var original = new CommandProposal([new CommandInstruction(1, [
            new CommandArgument(2, CommandValue.FromText(quoted)),
            new CommandArgument(3, CommandValue.FromText(path))
        ])]);
        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(original));
        Assert.Equal(quoted, decoded.Commands[0].Arguments[0].Value.Text);
        Assert.Equal(path, decoded.Commands[0].Arguments[1].Value.Text);
    }

    [Fact]
    public void EncodeThenDecode_PreservesQuotedTextAndBackslashes()
    {
        var quoted = """a "quoted" value with spaces""";
        var path = @"path\with\slashes";
        var original = new CommandProposal([new CommandInstruction(1, [
            new CommandArgument(2, CommandValue.FromText(quoted)),
            new CommandArgument(3, CommandValue.FromText(path))
        ])]);
        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(original));
        Assert.Equal(quoted, decoded.Commands[0].Arguments[0].Value.Text);
        Assert.Equal(path, decoded.Commands[0].Arguments[1].Value.Text);
    }

    [Theory]
    [InlineData("[CommandAi]\nProtocol=CommandAi/9\nCommand=100\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nCommand=0\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nCommand=100 200=0\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nUnexpected=value\nCommand=100\n[/CommandAi]")]
    public void Decode_RejectsInvalidEnvelopes(string text) =>
        Assert.ThrowsAny<Exception>(() => CommandAiTextCodec.Decode(text));

    [Fact]
    public void Instruction_RejectsDuplicateParameters() =>
        Assert.Throws<ArgumentException>(() => new CommandInstruction(1, [
            new CommandArgument(2, CommandValue.FromIdentifier(3)),
            new CommandArgument(2, CommandValue.FromIdentifier(4))]));

    [Fact]
    public void Proposal_DoesNotAllowEmptyCommands() =>
        Assert.Throws<ArgumentException>(() => new CommandProposal([]));
}

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

    [Theory]
    [InlineData("[CommandAi]\nProtocol=CommandAi/9\nCommand=100\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nCommand=0\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nCommand=100 200=0\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nUnexpected=value\nCommand=100\n[/CommandAi]")]
    public void Decode_RejectsInvalidEnvelopes(string text) =>
        Assert.ThrowsAny<Exception>(() => CommandAiTextCodec.Decode(text));

    [Fact]
    public void Decode_UsesFormatExceptionForAnEmptyProposal()
    {
        var text = "[CommandAi]\nProtocol=CommandAi/1\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_UsesFormatExceptionForDuplicateParameterIdentities()
    {
        var text = "[CommandAi]\nProtocol=CommandAi/1\nCommand=100 200=1 200=2\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_UsesFormatExceptionForInvalidLiteralValues()
    {
        var text = "[CommandAi]\nProtocol=CommandAi/1\nCommand=100 200=\"\"\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Instruction_RejectsDuplicateParameters() =>
        Assert.Throws<ArgumentException>(() => new CommandInstruction(1, [
            new CommandArgument(2, CommandValue.FromIdentifier(3)),
            new CommandArgument(2, CommandValue.FromIdentifier(4))]));

    [Fact]
    public void Proposal_DoesNotAllowEmptyCommands() =>
        Assert.Throws<ArgumentException>(() => new CommandProposal([]));

    [Fact]
    public void Proposal_RejectsMoreThanTheCommandLimit()
    {
        var commands = Enumerable.Range(1, CommandLimits.MaxCommands + 1)
            .Select(id => new CommandInstruction((ulong)id));
        Assert.Throws<ArgumentException>(() => new CommandProposal(commands));
    }

    [Fact]
    public void Instruction_RejectsMoreThanTheArgumentLimit()
    {
        var arguments = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand + 1)
            .Select(id => new CommandArgument((ulong)id, CommandValue.FromIdentifier(1)));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(1, arguments));
    }

    [Fact]
    public void Literal_RejectsTextAboveTheLimit()
    {
        var text = new string('x', CommandLimits.MaxLiteralCharacters + 1);
        Assert.Throws<ArgumentException>(() => CommandValue.FromText(text));
    }

    [Fact]
    public void Decode_RejectsEnvelopeAboveTheCharacterLimit()
    {
        var text = "[CommandAi]\nProtocol=CommandAi/1\nCommand=1 2=\"" +
            new string('x', CommandLimits.MaxEnvelopeCharacters) +
            "\"\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_RejectsTooManyCommands()
    {
        var lines = new List<string> { "[CommandAi]", "Protocol=CommandAi/1" };
        lines.AddRange(Enumerable.Repeat("Command=1", CommandLimits.MaxCommands + 1));
        lines.Add("[/CommandAi]");
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(string.Join("\n", lines)));
    }

    [Fact]
    public void Decode_RejectsTooManyArgumentsOnOneCommand()
    {
        var tokens = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand + 1)
            .Select(id => $"{id}=1");
        var text = "[CommandAi]\nProtocol=CommandAi/1\nCommand=1 " +
            string.Join(' ', tokens) + "\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void FluentBuilder_CreatesOrderedGenericProposal()
    {
        var proposal = new CommandProposalBuilder()
            .AddCommand(100, command => command
                .WithIdentifier(200, 302)
                .WithText(201, "Logs.Diagnostic Stream"))
            .AddCommand(101)
            .Build();

        Assert.Equal(2, proposal.Commands.Count);
        Assert.Equal(100UL, proposal.Commands[0].ActionId);
        Assert.Equal(302UL, proposal.Commands[0].Arguments[0].Value.Identifier);
        Assert.Equal("Logs.Diagnostic Stream", proposal.Commands[0].Arguments[1].Value.Text);
        Assert.Equal(101UL, proposal.Commands[1].ActionId);
        Assert.Equal(proposal.Commands[0].ActionId, CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(proposal)).Commands[0].ActionId);
    }

    [Fact]
    public void FluentBuilder_RejectsDuplicateParameterIdentities()
    {
        Assert.Throws<ArgumentException>(() => new CommandProposalBuilder()
            .AddCommand(1, command => command
                .WithIdentifier(2, 3)
                .WithText(2, "duplicate")));
    }

    [Fact]
    public void FluentBuilder_EnforcesProposalCommandLimit()
    {
        var builder = new CommandProposalBuilder();
        for (var i = 1; i <= CommandLimits.MaxCommands; i++)
            builder.AddCommand((ulong)i);
        Assert.Throws<InvalidOperationException>(() => builder.AddCommand(CommandLimits.MaxCommands + 1UL));
    }

    [Fact]
    public void FluentBuilder_BuildReturnsIndependentSnapshots()
    {
        var builder = new CommandProposalBuilder().AddCommand(1);
        var first = builder.Build();
        builder.AddCommand(2);
        var second = builder.Build();
        Assert.Single(first.Commands);
        Assert.Equal(2, second.Commands.Count);
    }

    [Fact]
    public void Proposal_RejectsAggregateLiteralTextAboveTheLimit()
    {
        var literal = new string('x', CommandLimits.MaxLiteralCharacters);
        var first = new CommandInstruction(1, Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand)
            .Select(id => new CommandArgument((ulong)id, CommandValue.FromText(literal))));
        var second = new CommandInstruction(2, [
            new CommandArgument(1, CommandValue.FromText(literal))
        ]);
        Assert.Throws<ArgumentException>(() => new CommandProposal([first, second]));
    }
}

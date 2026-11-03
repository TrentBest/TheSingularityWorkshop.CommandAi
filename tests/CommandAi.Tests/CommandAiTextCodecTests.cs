using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandAiTextCodecTests
{
    private static CommandInstruction Command(params CommandArgument[] arguments) => new(100, arguments);

    [Fact]
    public void EncodeThenDecode_PreservesOrderedTypedArguments()
    {
        var proposal = new CommandProposal([
            Command(new CommandArgument(200, CommandValue.FromIdentifier(300)),
                    new CommandArgument(201, CommandValue.FromText("Logs.Diagnostic Stream with \"quotes\" and \\slashes")))
        ]);
        var encoded = CommandAiTextCodec.Encode(proposal);
        Assert.Contains("Protocol=CommandAi/1", encoded);
        Assert.Contains("Command=100 200=300 201=", encoded);
        var decoded = CommandAiTextCodec.Decode(encoded);
        Assert.Equal("CommandAi/1", decoded.ProtocolVersion);
        var command = Assert.Single(decoded.Commands);
        Assert.Equal(100UL, command.ActionId);
        Assert.Equal(200UL, command.Arguments[0].ParameterId);
        Assert.Equal(300UL, command.Arguments[0].Value.Identifier);
        Assert.Equal("Logs.Diagnostic Stream with \"quotes\" and \\slashes", command.Arguments[1].Value.Text);
    }

    [Fact]
    public void Encode_UsesDeterministicLfLineEndings()
    {
        var proposal = new CommandProposal([new CommandInstruction(100)]);
        Assert.Equal("[CommandAi]\nProtocol=CommandAi/1\nCommand=100\n[/CommandAi]", CommandAiTextCodec.Encode(proposal));
    }

    [Theory]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 200=\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/9\\nCommand=100\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nUnexpected=value\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=0\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 0=2\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=0\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=\"unterminated\\n[/CommandAi]")]
    public void Decode_RejectsMalformedEnvelopes(string value) =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(value.Replace("\\n", "\n")));

    [Fact]
    public void Decode_RejectsDuplicateProtocolDeclarations() =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("[CommandAi]\\nProtocol=CommandAi/1\\nProtocol=CommandAi/1\\nCommand=100\\n[/CommandAi]".Replace("\\n", "\n")));

    [Fact]
    public void Decode_RejectsDuplicateParameters() =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=3 2=4\\n[/CommandAi]".Replace("\\n", "\n")));

    [Fact]
    public void Decode_RejectsEmptyCommandsAndInvalidVersion() =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("[CommandAi]\\nProtocol=CommandAi/1\\n[/CommandAi]".Replace("\\n", "\n")));

    [Fact]
    public void Decode_UsesArgumentExceptionForEmptyInput() =>
        Assert.Throws<ArgumentException>(() => CommandAiTextCodec.Decode(" "));

    [Fact]
    public void Proposal_RejectsDuplicateParameters() =>
        Assert.Throws<ArgumentException>(() => Command(new CommandArgument(2, CommandValue.FromIdentifier(3)), new CommandArgument(2, CommandValue.FromIdentifier(4))));

    [Fact]
    public void Proposal_RejectsEmptyAndExcessiveCommands()
    {
        Assert.Throws<ArgumentException>(() => new CommandProposal([]));
        Assert.Throws<ArgumentException>(() => new CommandProposal(Enumerable.Range(1, CommandLimits.MaxCommands + 1).Select(i => new CommandInstruction((ulong)i))));
    }

    [Fact]
    public void Proposal_RejectsNullCommandsWithArgumentException()
    {
        var commands = new CommandInstruction[] { null! };
        var exception = Assert.Throws<ArgumentException>(() => new CommandProposal(commands));
        Assert.Equal("commands", exception.ParamName);
    }

    [Fact]
    public void Instruction_RejectsExcessiveArguments()
    {
        var args = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand + 1).Select(i => new CommandArgument((ulong)i, CommandValue.FromIdentifier(1)));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(1, args));
    }

    [Fact]
    public void Literal_RejectsTextAboveLimit() =>
        Assert.Throws<ArgumentException>(() => CommandValue.FromText(new string('x', CommandLimits.MaxLiteralCharacters + 1)));

    [Fact]
    public void Proposal_RejectsAggregateLiteralTextAboveLimit()
    {
        var literal = new string('x', CommandLimits.MaxLiteralCharacters);
        var args = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand).Select(i => new CommandArgument((ulong)i, CommandValue.FromText(literal)));
        var first = new CommandInstruction(1, args);
        var second = new CommandInstruction(2, args.Select(a => new CommandArgument(a.ParameterId, a.Value)));
        Assert.Throws<ArgumentException>(() => new CommandProposal([first, second]));
    }

    [Fact]
    public void Builder_CreatesImmutableSnapshot()
    {
        var builder = new CommandProposalBuilder().AddCommand(100, c => c.WithIdentifier(200, 300).WithText(201, "hello"));
        var proposal = builder.Build();
        Assert.Single(proposal.Commands);
        Assert.Equal("hello", proposal.Commands[0].Arguments[1].Value.Text);
    }

    [Theory]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=\"valid\"trailing\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=\"invalid\\q\"\\n[/CommandAi]")]
    [InlineData("[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100 2=\"   \"\\n[/CommandAi]")]
    public void Decode_RejectsInvalidLiteralRepresentations(string value) =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(value.Replace("\\n", "\n")));

    [Fact]
    public void Decode_RejectsUnexpectedContentAndNestedEnvelope()
    {
        var trailing = "[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100\\n[/CommandAi]\\nextra".Replace("\\n", "\n");
        var nested = "[CommandAi]\\n[CommandAi]\\nProtocol=CommandAi/1\\nCommand=100\\n[/CommandAi]\\n[/CommandAi]".Replace("\\n", "\n");
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(trailing));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(nested));
    }

    [Fact]
    public void Decode_RejectsEnvelopeAboveConfiguredCharacterLimit()
    {
        var oversized = "[CommandAi]\\n" + new string('x', CommandLimits.MaxEnvelopeCharacters) + "\\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(oversized));
    }

    [Fact]
    public void Encode_RejectsProposalWhoseEncodedEnvelopeExceedsLimit()
    {
        var text = new string('x', CommandLimits.MaxLiteralCharacters);
        var arguments = Enumerable.Range(1, 16)
            .Select(i => new CommandArgument((ulong)i, CommandValue.FromText(text)));
        var proposal = new CommandProposal([new CommandInstruction(1, arguments)]);
        Assert.Throws<ArgumentException>(() => CommandAiTextCodec.Encode(proposal));
    }

    [Fact]
    public void Proposal_AcceptsConfiguredCommandAndArgumentBoundaries()
    {
        var command = new CommandInstruction(1, Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand)
            .Select(i => new CommandArgument((ulong)i, CommandValue.FromIdentifier(1))));
        var proposal = new CommandProposal(Enumerable.Range(1, CommandLimits.MaxCommands)
            .Select(i => new CommandInstruction((ulong)i)));
        Assert.Equal(CommandLimits.MaxArgumentsPerCommand, command.Arguments.Count);
        Assert.Equal(CommandLimits.MaxCommands, proposal.Commands.Count);
    }

}

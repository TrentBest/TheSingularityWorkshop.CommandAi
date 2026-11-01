using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandAiTextCodecTests
{
    private static CommandInstruction CreateCommand(
        Guid? commandId = null,
        string? idempotencyKey = null,
        IEnumerable<CommandArgument>? arguments = null,
        IEnumerable<CommandPrecondition>? preconditions = null)
        => new(
            commandId ?? Guid.NewGuid(),
            100,
            CommandValue.FromIdentifier(300),
            CommandValue.FromText("workspace"),
            idempotencyKey ?? Guid.NewGuid().ToString("N"),
            new CommandOutcomeContract(400, [
                new CommandOutcomeField(401, CommandValueKind.LiteralText),
                new CommandOutcomeField(402, CommandValueKind.IntegerIdentifier, required: false)
            ]),
            arguments,
            preconditions);

    [Fact]
    public void EncodeThenDecode_PreservesCompleteAtomicCommandContract()
    {
        var commandId = Guid.NewGuid();
        var original = new CommandProposal([
            CreateCommand(
                commandId,
                "inspect:resource:42",
                [
                    new CommandArgument(200, CommandValue.FromIdentifier(302)),
                    new CommandArgument(201, CommandValue.FromText("Logs.Diagnostic Stream"))
                ],
                [new CommandPrecondition("GrammarAi/1", "resource.exists && resource.readable")])
        ]);

        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(original));
        var command = Assert.Single(decoded.Commands);

        Assert.Equal("CommandAi/2", decoded.ProtocolVersion);
        Assert.Equal(commandId, command.CommandId);
        Assert.Equal(100UL, command.ActionId);
        Assert.Equal(300UL, command.Target.Identifier);
        Assert.Equal("workspace", command.Scope.Text);
        Assert.Equal("inspect:resource:42", command.IdempotencyKey);
        Assert.Equal(2, command.Arguments.Count);
        Assert.Equal(302UL, command.Arguments[0].Value.Identifier);
        Assert.Equal("Logs.Diagnostic Stream", command.Arguments[1].Value.Text);
        Assert.Equal("GrammarAi/1", Assert.Single(command.Preconditions).GrammarVersion);
        Assert.Equal("resource.exists && resource.readable", command.Preconditions[0].Expression);
        Assert.Equal(400UL, command.OutcomeContract.OutcomeId);
        Assert.Equal(2, command.OutcomeContract.Fields.Count);
        Assert.True(command.OutcomeContract.Fields[0].Required);
        Assert.False(command.OutcomeContract.Fields[1].Required);
    }

    [Fact]
    public void EncodeThenDecode_PreservesJsonEscapesInOpaqueText()
    {
        var expression = """resource.name == "a quoted value" && path == "c:\\temp" """;
        var command = CreateCommand(preconditions: [new CommandPrecondition("GrammarAi/1", expression)]);
        var decoded = CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(new CommandProposal([command])));
        Assert.Equal(expression, Assert.Single(Assert.Single(decoded.Commands).Preconditions).Expression);
    }

    [Theory]
    [InlineData("[CommandAi]\nProtocol=CommandAi/1\nCommand=100\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/9\nCommand={}\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/2\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/2\nUnexpected=value\n[/CommandAi]")]
    [InlineData("[CommandAi]\nProtocol=CommandAi/2\nCommand={bad json}\n[/CommandAi]")]
    public void Decode_RejectsUnsupportedOrMalformedEnvelopes(string text) =>
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));

    [Fact]
    public void Decode_UsesArgumentExceptionWhenNoInputWasSupplied() =>
        Assert.Throws<ArgumentException>(() => CommandAiTextCodec.Decode(" "));

    [Fact]
    public void Decode_RejectsDuplicateProtocolDeclarations()
    {
        var text = "[CommandAi]\nProtocol=CommandAi/2\nProtocol=CommandAi/2\nCommand={}\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_RejectsUnknownJsonProperties()
    {
        var json = """{"commandId":"00000000-0000-0000-0000-000000000001","actionId":1,"target":{"kind":"IntegerIdentifier","identifier":1,"text":null},"scope":{"kind":"IntegerIdentifier","identifier":2,"text":null},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[]},"arguments":[],"preconditions":[],"unexpected":true}""";
        var text = $"[CommandAi]\nProtocol=CommandAi/2\nCommand={json}\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_RejectsDuplicateJsonPropertiesAtAnyLevel()
    {
        var json = """{"commandId":"00000000-0000-0000-0000-000000000001","commandId":"00000000-0000-0000-0000-000000000002","actionId":1,"target":{"kind":"IntegerIdentifier","identifier":1,"text":null},"scope":{"kind":"IntegerIdentifier","identifier":2,"text":null},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[]},"arguments":[],"preconditions":[]}""";
        var text = $"[CommandAi]\\nProtocol=CommandAi/2\\nCommand={json}\\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Decode_RejectsOutcomeFieldWhenRequiredFlagIsMissing()
    {
        var json = """{"commandId":"00000000-0000-0000-0000-000000000001","actionId":1,"target":{"kind":"IntegerIdentifier","identifier":1,"text":null},"scope":{"kind":"IntegerIdentifier","identifier":2,"text":null},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[{"fieldId":2,"valueKind":"LiteralText"}]},"arguments":[],"preconditions":[]}""";
        var text = $"[CommandAi]\\nProtocol=CommandAi/2\\nCommand={json}\\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Proposal_RejectsAggregateLiteralTextAboveTheLimit()
    {
        var literal = new string('x', CommandLimits.MaxLiteralCharacters);
        var arguments = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand)
            .Select(id => new CommandArgument((ulong)id, CommandValue.FromText(literal)));
        var command = CreateCommand(arguments: arguments);
        Assert.Throws<ArgumentException>(() => new CommandProposal([command]));
    }

    [Fact]
    public void Instruction_RejectsDuplicateParameters() =>
        Assert.Throws<ArgumentException>(() => CreateCommand(arguments: [
            new CommandArgument(2, CommandValue.FromIdentifier(3)),
            new CommandArgument(2, CommandValue.FromIdentifier(4))]));

    [Fact]
    public void Proposal_RejectsDuplicateCommandIdentities()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => new CommandProposal([
            CreateCommand(commandId: id),
            CreateCommand(commandId: id)
        ]));
    }

    [Fact]
    public void Proposal_RejectsDuplicateIdempotencyKeys()
    {
        Assert.Throws<ArgumentException>(() => new CommandProposal([
            CreateCommand(idempotencyKey: "same"),
            CreateCommand(idempotencyKey: "same")
        ]));
    }

    [Fact]
    public void Proposal_DoesNotAllowEmptyCommands() =>
        Assert.Throws<ArgumentException>(() => new CommandProposal([]));

    [Fact]
    public void Proposal_RejectsMoreThanTheCommandLimit()
    {
        var commands = Enumerable.Range(1, CommandLimits.MaxCommands + 1)
            .Select(_ => CreateCommand());
        Assert.Throws<ArgumentException>(() => new CommandProposal(commands));
    }

    [Fact]
    public void Instruction_RejectsMoreThanTheArgumentLimit()
    {
        var arguments = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand + 1)
            .Select(id => new CommandArgument((ulong)id, CommandValue.FromIdentifier(1)));
        Assert.Throws<ArgumentException>(() => CreateCommand(arguments: arguments));
    }

    [Fact]
    public void Instruction_RejectsMoreThanThePreconditionLimit()
    {
        var preconditions = Enumerable.Range(1, CommandLimits.MaxPreconditionsPerCommand + 1)
            .Select(_ => new CommandPrecondition("GrammarAi/1", "true"));
        Assert.Throws<ArgumentException>(() => CreateCommand(preconditions: preconditions));
    }

    [Fact]
    public void OutcomeContract_RejectsDuplicateFields()
    {
        Assert.Throws<ArgumentException>(() => new CommandOutcomeContract(1, [
            new CommandOutcomeField(2, CommandValueKind.LiteralText),
            new CommandOutcomeField(2, CommandValueKind.IntegerIdentifier)
        ]));
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
        var text = "[CommandAi]\nProtocol=CommandAi/2\nCommand=" +
            new string('x', CommandLimits.MaxEnvelopeCharacters) +
            "\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(text));
    }

    [Fact]
    public void Encode_RejectsProposalThatExceedsTheEnvelopeLimit()
    {
        var literal = new string('x', CommandLimits.MaxLiteralCharacters);
        var arguments = Enumerable.Range(1, 17)
            .Select(id => new CommandArgument((ulong)id, CommandValue.FromText(literal)));
        var command = CreateCommand(arguments: arguments);
        var proposal = new CommandProposal([command]);
        Assert.Throws<ArgumentException>(() => CommandAiTextCodec.Encode(proposal));
    }

    [Fact]
    public void FluentBuilder_CreatesCompleteOrderedProposal()
    {
        var commandId = Guid.NewGuid();
        var outcome = new CommandOutcomeContract(400, [new CommandOutcomeField(401, CommandValueKind.LiteralText)]);
        var proposal = new CommandProposalBuilder()
            .AddCommand(commandId, 100, CommandValue.FromIdentifier(300), CommandValue.FromText("workspace"),
                "idempotency-1", outcome, command => command
                    .WithIdentifier(200, 302)
                    .WithText(201, "Logs.Diagnostic Stream")
                    .WithPrecondition("GrammarAi/1", "target.exists"))
            .Build();

        var command = Assert.Single(proposal.Commands);
        Assert.Equal(commandId, command.CommandId);
        Assert.Equal(100UL, command.ActionId);
        Assert.Single(command.Preconditions);
        Assert.Equal("idempotency-1", command.IdempotencyKey);
        var roundTripped = Assert.Single(CommandAiTextCodec.Decode(CommandAiTextCodec.Encode(proposal)).Commands);
        Assert.Equal(command.CommandId, roundTripped.CommandId);
        Assert.Equal(command.ActionId, roundTripped.ActionId);
        Assert.Equal(command.IdempotencyKey, roundTripped.IdempotencyKey);
        Assert.Equal(command.Target, roundTripped.Target);
        Assert.Equal(command.Scope, roundTripped.Scope);
        Assert.Equal(command.OutcomeContract.OutcomeId, roundTripped.OutcomeContract.OutcomeId);
        Assert.Equal(command.Arguments.Count, roundTripped.Arguments.Count);
        Assert.Equal(command.Preconditions[0].Expression, roundTripped.Preconditions[0].Expression);
    }

    [Fact]
    public void FluentBuilder_RejectsDuplicateParameterIdentities()
    {
        var builder = new CommandProposalBuilder();
        Assert.Throws<ArgumentException>(() => builder.AddCommand(
            Guid.NewGuid(), 1, CommandValue.FromIdentifier(2), CommandValue.FromIdentifier(3),
            "key", new CommandOutcomeContract(4),
            command => command.WithIdentifier(5, 6).WithText(5, "duplicate")));
    }

    [Fact]
    public void FluentBuilder_EnforcesProposalCommandLimit()
    {
        var builder = new CommandProposalBuilder();
        var outcome = new CommandOutcomeContract(4);
        for (var i = 1; i <= CommandLimits.MaxCommands; i++)
            builder.AddCommand(Guid.NewGuid(), (ulong)i, CommandValue.FromIdentifier(2),
                CommandValue.FromIdentifier(3), $"key-{i}", outcome);
        Assert.Throws<InvalidOperationException>(() => builder.AddCommand(
            Guid.NewGuid(), 999, CommandValue.FromIdentifier(2), CommandValue.FromIdentifier(3),
            "last", outcome));
    }

    [Fact]
    public void FluentBuilder_BuildReturnsIndependentSnapshots()
    {
        var builder = new CommandProposalBuilder();
        var outcome = new CommandOutcomeContract(4);
        builder.AddCommand(Guid.NewGuid(), 1, CommandValue.FromIdentifier(2),
            CommandValue.FromIdentifier(3), "first", outcome);
        var first = builder.Build();
        builder.AddCommand(Guid.NewGuid(), 2, CommandValue.FromIdentifier(2),
            CommandValue.FromIdentifier(3), "second", outcome);
        var second = builder.Build();
        Assert.Single(first.Commands);
        Assert.Equal(2, second.Commands.Count);
    }

    [Fact]
    public void Precondition_RejectsEmptyOrOversizedExpression()
    {
        Assert.Throws<ArgumentException>(() => new CommandPrecondition("GrammarAi/1", " "));
        Assert.Throws<ArgumentException>(() => new CommandPrecondition("GrammarAi/1",
            new string('x', CommandLimits.MaxPreconditionCharacters + 1)));
    }

    [Fact]
    public void Instruction_RequiresIdentityTargetScopeIdempotencyAndOutcome()
    {
        Assert.Throws<ArgumentException>(() => new CommandInstruction(
            Guid.Empty, 1, CommandValue.FromIdentifier(2), CommandValue.FromIdentifier(3),
            "key", new CommandOutcomeContract(4)));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(
            Guid.NewGuid(), 1, CommandValue.FromIdentifier(2), CommandValue.FromIdentifier(3),
            " ", new CommandOutcomeContract(4)));
    }
}

using TheSingularityWorkshop.CommandAi;

namespace CommandAi.Tests;

public sealed class CommandAiAtomicCommandTests
{
    private static CommandInstruction Enriched(
        Guid? commandId = null,
        string idempotencyKey = "idem-1",
        IEnumerable<CommandPrecondition>? preconditions = null)
        => new(
            commandId ?? Guid.Parse("11111111-2222-3333-4444-555555555555"),
            1001,
            CommandValue.FromText("TrentBest/Repository"),
            CommandValue.FromText("feature/isolated-work"),
            idempotencyKey,
            new CommandOutcomeContract(700, [
                new CommandOutcomeField(701, CommandValueKind.LiteralText),
                new CommandOutcomeField(702, CommandValueKind.IntegerIdentifier, required: false)
            ]),
            [
                new CommandArgument(2001, CommandValue.FromIdentifier(3001)),
                new CommandArgument(2002, CommandValue.FromText("bounded payload"))
            ],
            preconditions ?? [new CommandPrecondition("GrammarAi/Observer/1", "candidate.sha == expected.sha")]);

    [Fact]
    public void EnrichedAtomicCommand_RoundTripsAllGenericFieldsWithinCommandAi1()
    {
        var commandId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var command = Enriched(commandId);
        var encoded = CommandAiTextCodec.Encode(new CommandProposal([command]));

        Assert.Contains("Protocol=CommandAi/1", encoded);
        Assert.Contains("AtomicCommand=", encoded);
        Assert.DoesNotContain("\nCommand=", encoded);

        var decoded = Assert.Single(CommandAiTextCodec.Decode(encoded).Commands);
        Assert.True(decoded.HasAtomicContract);
        Assert.Equal(commandId, decoded.CommandId);
        Assert.Equal(1001UL, decoded.ActionId);
        Assert.Equal("TrentBest/Repository", decoded.Target!.Text);
        Assert.Equal("feature/isolated-work", decoded.Scope!.Text);
        Assert.Equal("idem-1", decoded.IdempotencyKey);
        Assert.Equal(700UL, decoded.OutcomeContract!.OutcomeId);
        Assert.Equal(701UL, decoded.OutcomeContract.Fields[0].FieldId);
        Assert.Equal("bounded payload", decoded.Arguments[1].Value.Text);
        Assert.Equal("GrammarAi/Observer/1", decoded.Preconditions[0].GrammarVersion);
        Assert.Equal("candidate.sha == expected.sha", decoded.Preconditions[0].Expression);
    }

    [Fact]
    public void MixedProposal_PreservesLegacyCompactAndEnrichedCommandsInOrder()
    {
        var legacy = new CommandInstruction(42, [
            new CommandArgument(5, CommandValue.FromIdentifier(9))
        ]);
        var enriched = Enriched(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"));
        var encoded = CommandAiTextCodec.Encode(new CommandProposal([legacy, enriched]));

        Assert.Contains("Command=42 5=9", encoded);
        Assert.Contains("AtomicCommand=", encoded);

        var decoded = CommandAiTextCodec.Decode(encoded);
        Assert.Equal(2, decoded.Commands.Count);
        Assert.False(decoded.Commands[0].HasAtomicContract);
        Assert.Null(decoded.Commands[0].CommandId);
        Assert.Null(decoded.Commands[0].Target);
        Assert.Null(decoded.Commands[0].Scope);
        Assert.Null(decoded.Commands[0].IdempotencyKey);
        Assert.Null(decoded.Commands[0].OutcomeContract);
        Assert.True(decoded.Commands[1].HasAtomicContract);
        Assert.Equal(42UL, decoded.Commands[0].ActionId);
        Assert.Equal(1001UL, decoded.Commands[1].ActionId);
    }

    [Fact]
    public void LegacyCompactEncoding_RemainsByteForByteUnchanged()
    {
        var proposal = new CommandProposal([new CommandInstruction(100)]);
        Assert.Equal(
            "[CommandAi]\nProtocol=CommandAi/1\nCommand=100\n[/CommandAi]",
            CommandAiTextCodec.Encode(proposal));
    }

    [Fact]
    public void AtomicCommandJson_RejectsUnknownAndDuplicateProperties()
    {
        var prefix = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=";
        var suffix = "\n[/CommandAi]";
        var valid = "{\"commandId\":\"11111111-2222-3333-4444-555555555555\",\"actionId\":7,\"target\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"repo\"},\"scope\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"branch\"},\"idempotencyKey\":\"key\",\"outcomeContract\":{\"outcomeId\":1,\"fields\":[]},\"arguments\":[],\"preconditions\":[]}";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + valid.Replace("\"actionId\":7", "\"actionId\":7,\"unknown\":true") + suffix));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + valid.Replace("\"actionId\":7", "\"actionId\":7,\"actionId\":8") + suffix));
    }

    [Fact]
    public void AtomicCommandJson_RejectsMissingRequiredFields()
    {
        const string missingTarget = "{\"commandId\":\"11111111-2222-3333-4444-555555555555\",\"actionId\":7,\"scope\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"branch\"},\"idempotencyKey\":\"key\",\"outcomeContract\":{\"outcomeId\":1,\"fields\":[]},\"arguments\":[],\"preconditions\":[]}";
        var input = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=" + missingTarget + "\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(input));
    }

    [Fact]
    public void Proposal_RejectsDuplicateEnrichedCommandAndIdempotencyIdentities()
    {
        var commandId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        Assert.Throws<ArgumentException>(() => new CommandProposal([
            Enriched(commandId, "idem-a"),
            Enriched(commandId, "idem-b")
        ]));
        Assert.Throws<ArgumentException>(() => new CommandProposal([
            Enriched(Guid.NewGuid(), "same-key"),
            Enriched(Guid.NewGuid(), "same-key")
        ]));
    }

    [Fact]
    public void EnrichedModel_EnforcesIdentityAndPreconditionBounds()
    {
        Assert.Throws<ArgumentException>(() => Enriched(idempotencyKey: new string('x', CommandLimits.MaxIdempotencyKeyCharacters + 1)));
        Assert.Throws<ArgumentException>(() => new CommandPrecondition(
            new string('g', CommandLimits.MaxGrammarVersionCharacters + 1), "condition"));
        Assert.Throws<ArgumentException>(() => new CommandPrecondition(
            "GrammarAi/1", new string('x', CommandLimits.MaxPreconditionCharacters + 1)));
        Assert.Throws<ArgumentException>(() => new CommandOutcomeContract(
            1, Enumerable.Range(1, CommandLimits.MaxOutcomeFields + 1)
                .Select(i => new CommandOutcomeField((ulong)i, CommandValueKind.LiteralText))));
    }

    [Fact]
    public void EnrichedBuilder_ComposesPreconditionsAndExpectedOutcome()
    {
        var proposal = new CommandProposalBuilder()
            .AddCommand(
                Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
                1001,
                CommandValue.FromText("repo"),
                CommandValue.FromText("feature/branch"),
                "idempotency-1",
                new CommandOutcomeContract(700, [new CommandOutcomeField(701, CommandValueKind.LiteralText)]),
                builder => builder
                    .WithText(2001, "file/path")
                    .WithPrecondition("GrammarAi/1", "file.sha == expected.sha"))
            .Build();

        var command = Assert.Single(proposal.Commands);
        Assert.True(command.HasAtomicContract);
        Assert.Equal("file/path", command.Arguments[0].Value.Text);
        Assert.Equal("GrammarAi/1", command.Preconditions[0].GrammarVersion);
    }

    [Fact]
    public void CompactBuilder_DoesNotInventAtomicContractDefaults()
    {
        var command = new CommandInstructionBuilder(100).WithText(200, "legacy").Build();
        Assert.False(command.HasAtomicContract);
        Assert.Null(command.CommandId);
        Assert.Null(command.Target);
        Assert.Null(command.Scope);
        Assert.Null(command.IdempotencyKey);
        Assert.Null(command.OutcomeContract);
        Assert.Empty(command.Preconditions);
        Assert.Throws<InvalidOperationException>(() =>
            new CommandInstructionBuilder(100).WithPrecondition("GrammarAi/1", "true"));
    }

    [Fact]
    public void Proposal_EnforcesAggregateLiteralBoundAcrossAtomicFields()
    {
        var oversized = new string('x', CommandLimits.MaxLiteralCharacters);
        var arguments = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand)
            .Select(i => new CommandArgument((ulong)i, CommandValue.FromText(oversized)));
        var preconditions = Enumerable.Range(1, CommandLimits.MaxPreconditionsPerCommand)
            .Select(_ => new CommandPrecondition("G", oversized));
        var command = new CommandInstruction(
            Guid.NewGuid(),
            1001,
            CommandValue.FromText("target"),
            CommandValue.FromText("scope"),
            "idempotency",
            new CommandOutcomeContract(700),
            arguments,
            preconditions);
        Assert.Throws<ArgumentException>(() => new CommandProposal([command]));
    }
}

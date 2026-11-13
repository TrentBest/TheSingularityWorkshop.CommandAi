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
            preconditions ?? [new CommandPrecondition("TheSingularityWorkshop.GrammarAi.Observer@1", "candidate.sha == expected.sha")]);

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
        Assert.Equal(CommandValueKind.LiteralText, decoded.OutcomeContract.Fields[0].ValueKind);
        Assert.True(decoded.OutcomeContract.Fields[0].Required);
        Assert.False(decoded.OutcomeContract.Fields[1].Required);
        Assert.Equal("bounded payload", decoded.Arguments[1].Value.Text);
        Assert.Equal("TheSingularityWorkshop.GrammarAi.Observer@1", decoded.Preconditions[0].GrammarVersion);
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
    public void AtomicCommandJson_RejectsOutcomeFieldMissingRequiredFlag()
    {
        const string missingRequired = "{\"commandId\":\"11111111-2222-3333-4444-555555555555\",\"actionId\":7,\"target\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"repo\"},\"scope\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"branch\"},\"idempotencyKey\":\"key\",\"outcomeContract\":{\"outcomeId\":1,\"fields\":[{\"fieldId\":2,\"valueKind\":\"LiteralText\"}]},\"arguments\":[],\"preconditions\":[]}";
        var input = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=" + missingRequired + "\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(input));
    }

    [Fact]
    public void EnrichedAtomicCommand_RoundTripsIntegerTargetAndScope()
    {
        var command = new CommandInstruction(
            Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            1002,
            CommandValue.FromIdentifier(3001),
            CommandValue.FromIdentifier(3002),
            "integer-target-operation",
            new CommandOutcomeContract(701, [new CommandOutcomeField(702, CommandValueKind.IntegerIdentifier)]));

        var decoded = Assert.Single(CommandAiTextCodec.Decode(
            CommandAiTextCodec.Encode(new CommandProposal([command]))).Commands);

        Assert.Equal(3001UL, decoded.Target!.Identifier);
        Assert.Equal(3002UL, decoded.Scope!.Identifier);
        Assert.Null(decoded.Target.Text);
        Assert.Null(decoded.Scope.Text);
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
            "TheSingularityWorkshop.GrammarAi.Observer@1", new string('x', CommandLimits.MaxPreconditionCharacters + 1)));
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
                    .WithPrecondition("TheSingularityWorkshop.GrammarAi.Observer@1", "file.sha == expected.sha"))
            .Build();

        var command = Assert.Single(proposal.Commands);
        Assert.True(command.HasAtomicContract);
        Assert.Equal("file/path", command.Arguments[0].Value.Text);
        Assert.Equal("TheSingularityWorkshop.GrammarAi.Observer@1", command.Preconditions[0].GrammarVersion);
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
            new CommandInstructionBuilder(100).WithPrecondition("TheSingularityWorkshop.GrammarAi.Observer@1", "true"));
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


    [Fact]
    public void AtomicCommandJson_RejectsNullNestedEntries()
    {
        const string prefix = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=";
        const string suffix = "\n[/CommandAi]";
        const string start = "{\"commandId\":\"11111111-2222-3333-4444-555555555555\",\"actionId\":7,\"target\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"repo\"},\"scope\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"branch\"},\"idempotencyKey\":\"key\",\"outcomeContract\":{\"outcomeId\":1,\"fields\":";
        const string end = "},\"arguments\":[],\"preconditions\":[]}";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + start + "[null]" + end + suffix));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + start + "[]" + end.Replace("\"arguments\":[]", "\"arguments\":[null]") + suffix));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + start + "[]" + end.Replace("\"preconditions\":[]", "\"preconditions\":[null]") + suffix));
    }

    [Fact]
    public void AtomicCommandJson_RejectsInvalidTypedValueFieldCombinations()
    {
        const string prefix = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=";
        const string suffix = "\n[/CommandAi]";
        const string invalid = "{\"commandId\":\"11111111-2222-3333-4444-555555555555\",\"actionId\":7,\"target\":{\"kind\":\"IntegerIdentifier\",\"identifier\":0,\"text\":\"unexpected\"},\"scope\":{\"kind\":\"LiteralText\",\"identifier\":null,\"text\":\"branch\"},\"idempotencyKey\":\"key\",\"outcomeContract\":{\"outcomeId\":1,\"fields\":[]},\"arguments\":[],\"preconditions\":[]}";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + invalid + suffix));
    }

    [Fact]
    public void EnrichedAtomicCommand_AcceptsExactCollectionBoundaries()
    {
        var arguments = Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand)
            .Select(i => new CommandArgument((ulong)i, CommandValue.FromIdentifier((ulong)i)));
        var fields = Enumerable.Range(1, CommandLimits.MaxOutcomeFields)
            .Select(i => new CommandOutcomeField((ulong)i, CommandValueKind.IntegerIdentifier));
        var preconditions = Enumerable.Range(1, CommandLimits.MaxPreconditionsPerCommand)
            .Select(i => new CommandPrecondition("G", $"condition-{i}"));
        var command = new CommandInstruction(
            Guid.NewGuid(),
            1001,
            CommandValue.FromIdentifier(3001),
            CommandValue.FromIdentifier(3002),
            "boundary-key",
            new CommandOutcomeContract(700, fields),
            arguments,
            preconditions);

        var decoded = Assert.Single(CommandAiTextCodec.Decode(
            CommandAiTextCodec.Encode(new CommandProposal([command]))).Commands);
        Assert.Equal(CommandLimits.MaxArgumentsPerCommand, decoded.Arguments.Count);
        Assert.Equal(CommandLimits.MaxOutcomeFields, decoded.OutcomeContract!.Fields.Count);
        Assert.Equal(CommandLimits.MaxPreconditionsPerCommand, decoded.Preconditions.Count);
    }


    [Fact]
    public void AtomicModel_ConstructorsRejectInvalidIdentitiesAndNullItems()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandInstruction(0));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(Guid.Empty, 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandInstruction(Guid.NewGuid(), 0,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentNullException>(() => new CommandInstruction(Guid.NewGuid(), 1,
            null!, CommandValue.FromIdentifier(2), "key", new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentNullException>(() => new CommandInstruction(Guid.NewGuid(), 1,
            CommandValue.FromIdentifier(1), null!, "key", new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(Guid.NewGuid(), 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), " ",
            new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentNullException>(() => new CommandInstruction(Guid.NewGuid(), 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key", null!));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(1,
            new CommandArgument[] { null! }));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(Guid.NewGuid(), 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1), preconditions: new CommandPrecondition[] { null! }));
    }

    [Fact]
    public void AtomicBuilders_EnforceArgumentAndPreconditionLimits()
    {
        var builder = new CommandInstructionBuilder(1);
        Assert.Throws<ArgumentNullException>(() => builder.WithValue(1, null!));
        builder.WithIdentifier(1, 1);
        Assert.Throws<ArgumentException>(() => builder.WithIdentifier(1, 2));

        var enriched = new CommandInstructionBuilder(Guid.NewGuid(), 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1));
        for (var i = 0; i < CommandLimits.MaxPreconditionsPerCommand; i++)
            enriched.WithPrecondition("G", $"condition-{i}");
        Assert.Throws<InvalidOperationException>(() => enriched.WithPrecondition("G", "overflow"));

        var full = new CommandInstructionBuilder(2);
        for (var i = 1; i <= CommandLimits.MaxArgumentsPerCommand; i++)
            full.WithIdentifier((ulong)i, (ulong)i);
        Assert.Throws<InvalidOperationException>(() => full.WithIdentifier(999, 999));
    }

    [Fact]
    public void OutcomeModels_RejectInvalidAndDuplicateFields()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandOutcomeContract(0));
        Assert.Throws<ArgumentException>(() => new CommandOutcomeContract(1,
            new CommandOutcomeField[] { null! }));
        Assert.Throws<ArgumentException>(() => new CommandOutcomeContract(1, [
            new CommandOutcomeField(7, CommandValueKind.LiteralText),
            new CommandOutcomeField(7, CommandValueKind.IntegerIdentifier)
        ]));
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandOutcomeField(0, CommandValueKind.LiteralText));
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandOutcomeField(1, (CommandValueKind)999));
    }

    [Fact]
    public void Preconditions_RejectBlankAndOversizedValues()
    {
        Assert.Throws<ArgumentException>(() => new CommandPrecondition(" ", "expression"));
        Assert.Throws<ArgumentException>(() => new CommandPrecondition("G", " "));
    }

    [Fact]
    public void ProposalBuilder_RejectsCommandBeyondConfiguredCapacity()
    {
        var builder = new CommandProposalBuilder();
        for (var i = 1; i <= CommandLimits.MaxCommands; i++)
            builder.AddCommand((ulong)i);
        Assert.Throws<InvalidOperationException>(() => builder.AddCommand(999));
    }


    [Fact]
    public void AtomicModel_RejectsTooManyPreconditionsAtConstruction()
    {
        var preconditions = Enumerable.Range(1, CommandLimits.MaxPreconditionsPerCommand + 1)
            .Select(i => new CommandPrecondition("G", $"condition-{i}"));
        Assert.Throws<ArgumentException>(() => new CommandInstruction(
            Guid.NewGuid(), 1, CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2),
            "key", new CommandOutcomeContract(1), preconditions: preconditions));
    }

    [Fact]
    public void AtomicBuilders_RejectInvalidConstructorArguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandInstructionBuilder(0));
        Assert.Throws<ArgumentException>(() => new CommandInstructionBuilder(Guid.Empty, 1,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandInstructionBuilder(Guid.NewGuid(), 0,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "key",
            new CommandOutcomeContract(1)));
    }

    [Fact]
    public void AtomicCodec_RejectsMalformedEnvelopeAndCompactArgumentOverflow()
    {
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("Protocol=CommandAi/1"));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("preamble\n[CommandAi]\nProtocol=CommandAi/1\nCommand=1\n[/CommandAi]"));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode("[CommandAi]\nProtocol=CommandAi/1"));
        var arguments = string.Join(" ", Enumerable.Range(1, CommandLimits.MaxArgumentsPerCommand + 1)
            .Select(i => $"{i}=1"));
        var tooManyArguments = $"[CommandAi]\nProtocol=CommandAi/1\nCommand=1 {arguments}\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(tooManyArguments));
    }

    [Fact]
    public void AtomicCodec_RejectsNonObjectJsonAndModelInvalidAtomicCommand()
    {
        var prefix = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=";
        var suffix = "\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + "[]" + suffix));

        var valid = CommandAiTextCodec.Encode(new CommandProposal([Enriched()]));
        var invalidIdentity = valid.Replace(
            "11111111-2222-3333-4444-555555555555",
            "00000000-0000-0000-0000-000000000000");
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(invalidIdentity));

        var nullArgumentValue = valid.Replace(
            "\"arguments\":[{\"parameterId\":2001,\"value\":{\"kind\":\"IntegerIdentifier\",\"identifier\":3001,\"text\":null}}",
            "\"arguments\":[{\"parameterId\":2001,\"value\":null}");
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(nullArgumentValue));
    }

    [Fact]
    public void GrammarBody_RoundTripsOpaqueBytesAndUnknownPositiveWireVersion()
    {
        var bytes = new byte[] { 0, 1, 2, 127, 128, 255 };
        var command = new CommandInstruction(Guid.NewGuid(), 1001,
            CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2), "grammar-body-roundtrip",
            new CommandOutcomeContract(700), grammarBody: new CommandGrammarBody(987, bytes));
        var decoded = Assert.Single(CommandAiTextCodec.Decode(
            CommandAiTextCodec.Encode(new CommandProposal([command]))).Commands);
        Assert.Equal(987, decoded.GrammarBody!.GrammarWireVersion);
        Assert.Equal(bytes, decoded.GrammarBody.Payload.ToArray());
    }

    [Fact]
    public void GrammarBody_RejectsInvalidVersionsEmptyAndOversizedPayloads()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CommandGrammarBody(0, new byte[] { 1 }));
        Assert.Throws<ArgumentException>(() => new CommandGrammarBody(1, Array.Empty<byte>()));
        Assert.Throws<ArgumentException>(() => new CommandGrammarBody(1,
            new byte[CommandLimits.MaxGrammarBodyBytesPerCommand + 1]));
    }

    [Fact]
    public void GrammarBody_RejectsNonCanonicalBase64AndProposalAggregateOverflow()
    {
        const string prefix = "[CommandAi]\nProtocol=CommandAi/1\nAtomicCommand=";
        const string suffix = "\n[/CommandAi]";
        const string json = """{"commandId":"11111111-2222-3333-4444-555555555555","actionId":7,"target":{"kind":"LiteralText","identifier":null,"text":"repo"},"scope":{"kind":"LiteralText","identifier":null,"text":"branch"},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[]},"arguments":[],"preconditions":[],"body":{"grammarWireVersion":1,"payloadBase64":"AQ=="}}""";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + json.Replace("AQ==", "AR==") + suffix));

        var commands = Enumerable.Range(0, 5).Select(i => new CommandInstruction(
            Guid.NewGuid(), (ulong)(100 + i), CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2),
            "aggregate-" + i, new CommandOutcomeContract(700),
            grammarBody: new CommandGrammarBody(1, new byte[CommandLimits.MaxGrammarBodyBytesPerCommand])));
        Assert.Throws<ArgumentException>(() => new CommandProposal(commands));
    }


    [Fact]
    public void GrammarBodyBuilder_RequiresEnrichedCommandAndNonNullBody()
    {
        var body = new CommandGrammarBody(1, new byte[] { 1, 2, 3 });
        Assert.Throws<InvalidOperationException>(() =>
            new CommandInstructionBuilder(100).WithGrammarBody(body));

        var builder = new CommandInstructionBuilder(
            Guid.NewGuid(), 100, CommandValue.FromIdentifier(1), CommandValue.FromIdentifier(2),
            "grammar-body-builder", new CommandOutcomeContract(1));
        Assert.Throws<ArgumentNullException>(() => builder.WithGrammarBody(null!));

        var command = builder.WithGrammarBody(body).Build();
        Assert.Same(body, command.GrammarBody);
    }

    [Fact]
    public void GrammarBodyDecoder_RejectsEmptyMalformedAndInvalidVersionFields()
    {
        const string prefix = "[CommandAi]\\nProtocol=CommandAi/1\\nAtomicCommand=";
        const string suffix = "\\n[/CommandAi]";
        const string json = """{"commandId":"11111111-2222-3333-4444-555555555555","actionId":7,"target":{"kind":"LiteralText","identifier":null,"text":"repo"},"scope":{"kind":"LiteralText","identifier":null,"text":"branch"},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[]},"arguments":[],"preconditions":[],"body":{"grammarWireVersion":1,"payloadBase64":"AQ=="}}""";

        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + json.Replace("\\"payloadBase64\\":\\"AQ==\\"", "\\"payloadBase64\\":\\"\\"") + suffix));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + json.Replace("AQ==", "!") + suffix));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(prefix + json.Replace("\\"grammarWireVersion\\":1", "\\"grammarWireVersion\\":0") + suffix));
    }

    [Fact]
    public void AtomicCodec_RejectsNestedEnvelopeAndExcessiveEntryCount()
    {
        var nested = "[CommandAi]\\n[CommandAi]\\nProtocol=CommandAi/1\\nCommand=1\\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(nested));

        var excessive = string.Join("\\n",
            new[] { "[CommandAi]", "Protocol=CommandAi/1" }
                .Concat(Enumerable.Repeat("Unexpected=entry", CommandLimits.MaxCommands + 1))
                .Append("[/CommandAi]"));
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(excessive));
    }

    [Fact]
    public void AtomicCodec_RejectsNullTypedArgumentValue()
    {
        const string json = """{"commandId":"11111111-2222-3333-4444-555555555555","actionId":7,"target":{"kind":"LiteralText","identifier":null,"text":"repo"},"scope":{"kind":"LiteralText","identifier":null,"text":"branch"},"idempotencyKey":"key","outcomeContract":{"outcomeId":1,"fields":[]},"arguments":[{"parameterId":1,"value":null}],"preconditions":[]}""";
        var input = "[CommandAi]\\nProtocol=CommandAi/1\\nAtomicCommand=" + json + "\\n[/CommandAi]";
        Assert.Throws<FormatException>(() => CommandAiTextCodec.Decode(input));
    }
}

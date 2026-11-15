<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Construct a compact proposal

~~~csharp
using TheSingularityWorkshop.CommandAi;

var compactProposal = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(parameterId: 200, valueId: 42)
        .WithText(parameterId: 201, value: "diagnostic stream"))
    .Build();
~~~

The numeric IDs are examples. The consuming host defines their meanings and supplies trusted mappings. CommandAi treats them as opaque identities.

## Construct an enriched atomic command

An enriched instruction requires an explicit command identity, action identity, typed target and scope, caller-supplied idempotency identity, and expected-outcome contract. The builder does not infer these fields for compact commands.

~~~csharp
var outcome = new CommandOutcomeContract(
    outcomeId: 900,
    fields:
    [
        new CommandOutcomeField(901, CommandValueKind.LiteralText, required: true),
        new CommandOutcomeField(902, CommandValueKind.IntegerIdentifier, required: false)
    ]);

var proposal = new CommandProposalBuilder()
    .AddCommand(
        commandId: Guid.Parse("6d3c0d8f-72d4-4a92-9b80-4a5b5e0d0001"),
        actionId: 100,
        target: CommandValue.FromIdentifier(42),
        scope: CommandValue.FromText("workspace"),
        idempotencyKey: "request-2026-0001",
        outcomeContract: outcome,
        configure: command => command
            .WithIdentifier(parameterId: 200, valueId: 7)
            .WithPrecondition(
                "TheSingularityWorkshop.GrammarAi.Observer@1",
                "resource.isAvailable"))
    .Build();
~~~

The command identity and idempotency identity must be unique across enriched commands in one proposal. These are declarations for the host; they do not grant authority or enforce replay protection.

## Attach an opaque GrammarAi body

CommandAi does not parse a GrammarAi body. It transports bounded bytes with a positive grammar wire version:

~~~csharp
var body = new CommandGrammarBody(
    grammarWireVersion: 1,
    payload: new byte[] { 0x01, 0x02, 0x03 });

var enriched = new CommandProposalBuilder()
    .AddCommand(
        Guid.Parse("6d3c0d8f-72d4-4a92-9b80-4a5b5e0d0002"),
        actionId: 100,
        target: CommandValue.FromIdentifier(42),
        scope: CommandValue.FromText("workspace"),
        idempotencyKey: "request-2026-0002",
        outcomeContract: new CommandOutcomeContract(900),
        configure: command => command.WithGrammarBody(body))
    .Build();
~~~

The codec serializes the body as canonical Base64. The decoded payload is limited to 32 KiB per command and 128 KiB across a proposal. GrammarAi owns interpretation and structural validation.

## Encode and decode

~~~csharp
var wireText = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wireText);
~~~

The protocol identity remains CommandAi/1. Compact entries retain the original shape:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=42 201="diagnostic stream"
[/CommandAi]
~~~

Enriched entries use an explicit AtomicCommand= line followed by a bounded JSON object. Both forms may coexist in one proposal. A compact-only reader must reject AtomicCommand entries and be upgraded before consuming enriched proposals; it must not silently ignore or downgrade them.

## Model and ownership

- CommandProposal is a non-empty ordered immutable collection.
- CommandInstruction is either a compact action-plus-arguments instruction or a complete enriched atomic-command declaration.
- CommandArgument pairs a positive parameter identity with a typed CommandValue.
- CommandValue distinguishes integer identifiers from literal text.
- CommandPrecondition preserves a grammar-version token and expression without parsing or evaluation.
- CommandOutcomeContract and CommandOutcomeField describe an expected outcome shape, not an actual outcome.
- CommandGrammarBody carries opaque GrammarAi bytes; it is not a grammar parser.
- The host owns vocabulary meaning, semantic validation, live-state checks, policy, authorization, approval, idempotency/replay enforcement, dispatch, execution, and actual outcomes.

Successful decoding proves only that the input is structurally valid and within limits. It does not authorize any action.

## Failure behavior

- Empty caller input: ArgumentException.
- Malformed non-empty envelope, unsupported protocol, invalid identifiers, malformed JSON or Base64, duplicate declarations/properties, invalid typed values, or wire resource-limit violation: FormatException.
- Direct construction with invalid caller-supplied values: argument-validation exceptions.
- Encoding a valid in-memory proposal that exceeds the envelope limit: ArgumentException.

## Verify

~~~bash
dotnet restore TheSingularityWorkshop.CommandAi.slnx
dotnet build TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet test TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

CI additionally packs both packages, audits their dependency boundaries, and compiles/runs package-only consumers. NuGet publication remains disabled.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
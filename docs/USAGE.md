<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Construct a compact legacy proposal

~~~csharp
using TheSingularityWorkshop.CommandAi;

var compact = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(parameterId: 200, valueId: 42)
        .WithText(parameterId: 201, value: "diagnostic stream"))
    .Build();
~~~

The numeric IDs are examples. The consuming host defines their meanings and supplies trusted mappings. Compact commands preserve the original /1 action-plus-arguments representation.

## Construct an enriched atomic command

~~~csharp
var outcome = new CommandOutcomeContract(700, [
    new CommandOutcomeField(701, CommandValueKind.LiteralText),
    new CommandOutcomeField(702, CommandValueKind.IntegerIdentifier, required: false)
]);

var proposal = new CommandProposalBuilder()
    .AddCommand(
        commandId: Guid.NewGuid(),
        actionId: 100,
        target: CommandValue.FromText("repository"),
        scope: CommandValue.FromText("feature/isolated-work"),
        idempotencyKey: "operation-001",
        outcomeContract: outcome,
        configure: command => command
            .WithIdentifier(parameterId: 200, valueId: 42)
            .WithText(parameterId: 201, value: "diagnostic stream")
            .WithPrecondition("GrammarAi/Observer/1", "candidate.sha == expected.sha"))
    .Build();
~~~

The fields are generic opaque data. CommandAi does not know what the repository, branch, action, outcome, or precondition means. GrammarAi owns the named grammar and validates its structure; the host evaluates it against live trusted state.

## Encode and decode

~~~csharp
var wireText = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wireText);
~~~

The protocol identifier remains CommandAi/1. Compact commands use the original Command line:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=42 201="diagnostic stream"
[/CommandAi]
~~~

Enriched commands use an AtomicCommand JSON line:

~~~text
[CommandAi]
Protocol=CommandAi/1
AtomicCommand={"commandId":"11111111-2222-3333-4444-555555555555","actionId":100,"target":{"kind":"LiteralText","identifier":null,"text":"repository"},"scope":{"kind":"LiteralText","identifier":null,"text":"feature/isolated-work"},"idempotencyKey":"operation-001","outcomeContract":{"outcomeId":700,"fields":[]},"arguments":[],"preconditions":[]}
[/CommandAi]
~~~

A proposal can mix compact and enriched commands. The new decoder reads both. Older compact-only readers reject AtomicCommand as an unknown envelope entry and must be upgraded before consuming enriched proposals.

## Model and ownership

- CommandProposal is a non-empty ordered collection; it is not a transaction.
- CommandInstruction always has an action ID and ordered arguments. Its HasAtomicContract property distinguishes enriched commands from compact legacy commands.
- CommandPrecondition stores grammar-versioned opaque data; it is not parsed or evaluated by CommandAi.
- CommandOutcomeContract declares an expected shape, not an actual result.
- CommandAi carries an idempotency identity but does not implement deduplication or replay.
- The host owns vocabulary meaning, semantic validation, live-state checks, policy, authorization, approval, execution, and actual outcomes.

Successful decoding proves only that the input is structurally valid and within limits. It does not authorize any action.

## Failure behavior

- Empty caller input: ArgumentException.
- Non-empty malformed envelope, unsupported protocol, invalid identifiers, malformed JSON, unknown/duplicate JSON properties, duplicate command identities/idempotency keys, duplicate parameters, or resource-limit violation: FormatException.
- Direct construction with invalid caller-supplied values: ArgumentException.
- Encoding a valid in-memory proposal that exceeds the envelope limit: ArgumentException.

## Verify

~~~bash
dotnet restore TheSingularityWorkshop.CommandAi.slnx
dotnet build TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet test TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

CI additionally packs the library and audits/compiles the quickstart against the produced package artifact. NuGet publication remains disabled.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Construct a proposal

~~~csharp
using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(parameterId: 200, valueId: 42)
        .WithText(parameterId: 201, value: "diagnostic stream"))
    .Build();
~~~

The numeric IDs are examples. The consuming host defines their meanings and supplies trusted mappings. CommandAi treats them as opaque identities.

## Encode and decode

~~~csharp
var wireText = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wireText);
~~~

The only approved wire contract is CommandAi/1:

~~~text
[CommandAi]
Protocol=CommandAi/1
Command=100 200=42 201="diagnostic stream"
[/CommandAi]
~~~

Each command line starts with a positive action ID, followed by zero or more ordered parameter/value pairs. Values are positive integer identifiers or JSON-quoted literal strings.

## Enriched atomic commands

The original compact constructor and `Command=` wire entry remain available. When a consumer needs a generic atomic-command declaration, use the enriched builder overload:

~~~csharp
var proposal = new CommandProposalBuilder()
    .AddCommand(
        commandId: Guid.NewGuid(),
        actionId: 1001,
        target: CommandValue.FromText("repository-id"),
        scope: CommandValue.FromText("isolated-feature-branch"),
        idempotencyKey: "stable-operation-key",
        outcomeContract: new CommandOutcomeContract(700, [
            new CommandOutcomeField(701, CommandValueKind.LiteralText)
        ]),
        configure: command => command
            .WithText(parameterId: 2001, value: "src/file.cs")
            .WithPrecondition("GrammarAi/1", "candidate.sha == expected.sha"))
    .Build();
~~~

The codec writes enriched commands as `AtomicCommand=` JSON entries under the same `CommandAi/1` envelope. New readers accept compact, enriched, and mixed proposals in order. Older compact-only readers reject enriched entries and must be upgraded before consuming them.

An enriched atomic command may also carry an optional opaque GrammarAi body. Attach it with `CommandInstructionBuilder.WithGrammarBody(new CommandGrammarBody(grammarWireVersion, payloadBytes))`. The wire member is `body: { grammarWireVersion, payloadBase64 }`; Base64 preserves the exact GrammarAi wire bytes and is only a JSON transport encoding. CommandAi requires a positive version, canonical Base64, a non-empty payload, at most 32 KiB decoded bytes per command, and at most 128 KiB decoded bytes across a proposal. It deliberately accepts unknown positive GrammarAi wire versions and does not decode or validate the body. The trusted consumer maps the action to the exact GrammarAi definition/schema version and delegates structural validation to GrammarAi. Hosts still own live-state checks, capability, policy, authorization, and execution. Older enriched readers that do not recognize `body` may reject body-bearing records; upgrade readers before sending them.

## Model and ownership

- `CommandProposal` is a non-empty ordered collection; it is not a transaction.
- `CommandInstruction` represents either the original compact action/argument shape or a complete enriched atomic-command declaration.
- `CommandArgument` pairs a positive parameter ID with a typed `CommandValue`.
- `CommandPrecondition` carries an opaque expression and GrammarAi grammar version; CommandAi does not parse or evaluate it.
- `CommandOutcomeContract` declares expected outcome shape, not an actual result.
- The host owns domain meaning, live-state evaluation, replay/deduplication enforcement, policy, authorization, approval, execution, and actual outcomes.

Successful decoding proves only that the input is structurally valid and within limits. It does not authorize any action. Specialized or domain-specific fields belong in consuming-domain extensions when they are not broadly applicable to the generic contract.

## Host-owned named operations

A consuming application can expose methods such as `InspectResource(...)` that map domain concepts to action and parameter IDs. These names and mappings are not built into CommandAi. See [Builder integration](BUILDER_INTEGRATION.md) and the [quickstart](../examples/CommandAi.QuickStart/Program.cs).

## Failure behavior

- Empty caller input: `ArgumentException`.
- Non-empty malformed envelope, unsupported protocol, invalid identifiers, malformed quoted literal, duplicate declarations/parameters, or wire resource-limit violation: `FormatException`.
- Direct construction with invalid caller-supplied values: `ArgumentException`.
- Encoding a valid in-memory proposal that exceeds the envelope limit: `ArgumentException`.

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

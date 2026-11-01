<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Construct a complete atomic command

~~~csharp
using TheSingularityWorkshop.CommandAi;

var commandId = Guid.NewGuid();
var outcome = new CommandOutcomeContract(400, [
    new CommandOutcomeField(401, CommandValueKind.LiteralText),
    new CommandOutcomeField(402, CommandValueKind.IntegerIdentifier, required: false)
]);

var proposal = new CommandProposalBuilder()
    .AddCommand(
        commandId,
        actionId: 100,
        target: CommandValue.FromIdentifier(300),
        scope: CommandValue.FromText("workspace"),
        idempotencyKey: "inspect:resource:42",
        outcomeContract: outcome,
        configure: command => command
            .WithIdentifier(parameterId: 200, valueId: 42)
            .WithPrecondition("GrammarAi/1", "resource.exists && resource.readable"))
    .Build();
~~~

The identities and expression are opaque to CommandAi. The consuming application owns the meanings of action, target, scope, parameter, outcome, and idempotency key. GrammarAi owns the grammar associated with the precondition expression.

## Encode and decode

~~~csharp
var wireText = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wireText);
~~~

The current wire contract is CommandAi/2. It serializes each complete command as a JSON object on a Command= line. JSON property names and value kinds are explicit; unknown properties and malformed contract fields are rejected. CommandAi/1 is a breaking predecessor and is not accepted by this decoder.

The codec validates representation only. It does not evaluate preconditions, enforce idempotency, resolve identities, authorize, or execute.

## Understand the model

- CommandInstruction.CommandId identifies this proposal instruction. It is not the idempotency key.
- Target and Scope are required opaque CommandValue instances.
- Arguments carry ordered parameter IDs and typed values.
- Preconditions carry an expression plus its grammar version. They are preserved, not interpreted.
- IdempotencyKey is required and bounded. A host must persist and enforce its duplicate-delivery policy.
- OutcomeContract describes the expected outcome shape. It is not the actual result of execution.
- CommandProposal is an ordered batch, not a transaction.

## Host-owned named operations

A consuming host may expose named extensions such as InspectResource(...) above CommandProposalBuilder. The host owns the names, IDs, semantic validation, target/scope selection, idempotency policy, and outcome definitions. See [Builder integration](BUILDER_INTEGRATION.md) and the [quickstart](../examples/CommandAi.QuickStart/Program.cs).

## Failure behavior

- Empty caller input: ArgumentException.
- Non-empty malformed envelope, unsupported version, malformed JSON, unknown JSON properties, missing required fields, invalid identifiers, duplicate command/parameter identities, or resource-limit violations while decoding: FormatException.
- Direct construction with invalid caller-supplied values: ArgumentException.
- Encoding a valid in-memory proposal that exceeds the envelope limit: ArgumentException.

## Verify the repository

~~~bash
dotnet restore TheSingularityWorkshop.CommandAi.slnx
dotnet build TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet test TheSingularityWorkshop.CommandAi.slnx --configuration Release
dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj
~~~

The GitHub Actions workflow also packs the library and compiles the quickstart against the produced package artifact. NuGet publication remains disabled.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

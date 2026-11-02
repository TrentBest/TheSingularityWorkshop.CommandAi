<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Construct a complete atomic command

~~~csharp
using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(parameterId: 200, valueId: 42)
        .WithText(parameterId: 201, value: "workspace"))
    .Build();
~~~

The identities and expression are opaque to CommandAi. The consuming application owns the meanings of action, target, scope, parameter, outcome, and idempotency key. GrammarAi owns the grammar associated with the precondition expression.

## Encode and decode

~~~csharp
var wireText = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wireText);
~~~

The current wire contract is CommandAi/1. Earlier alpha consumers should follow the [migration guide](MIGRATION_COMMAND_AI_2.md). Each Command= line contains an action identifier followed by parameter=value tokens. Integer identifiers are positive decimal numbers; literal values are JSON-quoted strings.

The codec validates representation only. It does not evaluate preconditions, enforce idempotency, resolve identities, authorize, or execute.

## Understand the model

- CommandInstruction contains a positive action identifier and ordered arguments.
- Each argument pairs a positive parameter identifier with an integer identifier or literal text.
- The host resolves identifiers and owns semantic validation, authorization, and execution.
- CommandProposal is an ordered batch, not a transaction.

## Host-owned named operations

A consuming host may expose named extensions such as InspectResource(...) above CommandProposalBuilder. The host owns the names, IDs, semantic validation, target/scope selection, idempotency policy, and outcome definitions. See [Builder integration](BUILDER_INTEGRATION.md) and the [quickstart](../examples/CommandAi.QuickStart/Program.cs).

## Failure behavior

- Empty caller input: ArgumentException.
- Non-empty malformed envelope, unsupported version, malformed JSON, invalid identifiers, malformed quoted literals, duplicate parameters, or resource-limit violations while decoding: FormatException.
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

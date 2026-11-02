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

## Model and ownership

- `CommandProposal` is a non-empty ordered collection; it is not a transaction.
- `CommandInstruction` contains a positive action ID and ordered arguments.
- `CommandArgument` pairs a positive parameter ID with a typed `CommandValue`.
- `CommandValue` distinguishes integer identifiers from literal text.
- The host owns vocabulary meaning, semantic validation, live-state checks, policy, authorization, approval, execution, and actual outcomes.

Successful decoding proves only that the input is structurally valid and within limits. It does not authorize any action.

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

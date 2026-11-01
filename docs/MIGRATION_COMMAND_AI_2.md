<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Migrate alpha consumers to CommandAi/2

## Why this is a breaking change

CommandAi is an alpha package. Its earlier 0.1.0-alpha.1 contract represented a command as an action identifier plus arguments and used the CommandAi/1 line-token envelope. That shape omitted required atomic-command metadata. The corrected contract is CommandAi/2, with explicit command identity, target, scope, parameters, preconditions, idempotency key, and expected outcome contract.

The old alpha was not published to NuGet. Consumers using a local project reference or repository source should update directly to the new API. No legacy decoder or compatibility adapter is provided.

## API migration

| Earlier shape | CommandAi/2 replacement |
|---|---|
| CommandInstruction(actionId, arguments) | Supply commandId, actionId, target, scope, idempotencyKey, and outcomeContract, plus optional arguments/preconditions |
| AddCommand(actionId, configure) | AddCommand(commandId, actionId, target, scope, idempotencyKey, outcomeContract, configure) |
| Action/argument-only command | Complete atomic command contract |
| No precondition field | WithPrecondition(grammarVersion, expression) |
| No declared result shape | CommandOutcomeContract and CommandOutcomeField |
| Protocol=CommandAi/1 token format | Protocol=CommandAi/2 JSON command records |

## Minimal updated construction

~~~csharp
using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(
        commandId: Guid.NewGuid(),
        actionId: 1001,
        target: CommandValue.FromIdentifier(42),
        scope: CommandValue.FromText("resource"),
        idempotencyKey: "inspect-resource:42",
        outcomeContract: new CommandOutcomeContract(3001, [
            new CommandOutcomeField(3002, CommandValueKind.LiteralText)
        ]),
        configure: command => command
            .WithIdentifier(parameterId: 2001, valueId: 42)
            .WithPrecondition("GrammarAi/1", "resource.exists"))
    .Build();

var wire = CommandAiTextCodec.Encode(proposal);
var parsed = CommandAiTextCodec.Decode(wire);
~~~

## Migration responsibilities

- Assign stable command identities when creating each proposed command.
- Define target and scope values using application-owned opaque identifiers or literal text.
- Supply an idempotency key appropriate to the host's retry and duplicate-delivery policy. CommandAi carries the key but does not persist or enforce it.
- Define the expected outcome identity and typed fields. This is not an actual result or CI verdict.
- Tag precondition expressions with the GrammarAi grammar version that defines them. CommandAi stores the expression; GrammarAi owns grammar structure and the host evaluates it against live state.
- Update any serialized fixtures, test data, snapshots, or generated prompts to CommandAi/2. The v1 decoder is intentionally not supported.
- Update consumers to use the new model and codec together; do not translate a v1 action/argument record by guessing missing metadata.

## Package boundaries remain unchanged

CommandAi represents intent. ProtocolAi owns correlated, provenance-bearing events and actual outcomes. GrammarAi owns grammar definitions. CiAi owns CI evidence and verdicts. FSM_Foundry and the host own live-state evaluation, authorization, approval, durable observer lifecycle, scheduling, dispatch, and execution.

## Verification

Run the consuming repository's restore, build, tests, and integration tests after updating its CommandAi reference. The CommandAi repository workflow additionally runs the quickstart, packs the package, audits its contents, and compiles the quickstart against that package artifact. NuGet publication is not authorized by this migration.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

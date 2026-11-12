<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi builder integration

## Design

CommandAi provides compact and enriched immutable proposal forms. A consuming host supplies named operations, trusted identity mappings, domain validation, policy, and execution behavior.

    Host-owned named operation
       | action / target / scope / parameter vocabulary
       v
    CommandProposalBuilder
       | compact or enriched atomic command
       v
    Immutable CommandProposal
       |
       v
    CommandAiTextCodec (CommandAi/1)

## Compact example

~~~csharp
var compact = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(200, 42)
        .WithText(201, "diagnostic stream"))
    .Build();
~~~

## Enriched atomic-command example

~~~csharp
var enriched = new CommandProposalBuilder()
    .AddCommand(
        Guid.NewGuid(),
        100,
        CommandValue.FromText("repository"),
        CommandValue.FromText("feature/ref"),
        "operation-001",
        new CommandOutcomeContract(700, [
            new CommandOutcomeField(701, CommandValueKind.LiteralText)
        ]),
        command => command
            .WithIdentifier(200, 42)
            .WithPrecondition("GrammarAi/Observer/1", "candidate.sha == expected.sha"))
    .Build();
~~~

The generic model treats these values as opaque. The application owns the action vocabulary and meanings of target, scope, parameter, and outcome identities. GrammarAi owns precondition grammar and structural validation. The host evaluates preconditions against current trusted state.

## Extension boundary

A host may add named methods such as InspectResource or OpenDiagnosticStream. Such methods translate domain concepts into generic action, target, scope, and parameter values. They should validate their own inputs before constructing the proposal and retain authority to reject it later.

Construction and encoding do not authorize execution. The host resolves identities, validates current state, applies policy and approval requirements, dispatches the action, and records actual outcomes. The idempotency key is data only; the host must implement replay/deduplication semantics.

## Current ownership

| Capability | Owner |
|---|---|
| Command identity, action identity, target, scope, arguments | CommandAi |
| Grammar-versioned precondition data | CommandAi carries; GrammarAi validates structure; host evaluates live state |
| Expected-outcome declaration | CommandAi |
| Vocabulary meaning and domain identity mapping | ProtocolAi / consuming domain |
| Endpoint request/response transport | HeadlessAi |
| CI evidence and verdicts | CiAi |
| Authorization, approval, replay/delivery, dispatch, execution | FSM_Foundry / host |

Compact CommandAi/1 remains supported. Enriched AtomicCommand= entries are additive to the current reader, but older compact-only readers reject them; consumers must upgrade before emitting or accepting enriched entries.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

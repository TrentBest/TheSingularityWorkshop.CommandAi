<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi builder integration

## Design

CommandAi provides a generic immutable model and fluent construction surface. A consuming host supplies its own named operations, trusted identity mappings, domain validation, policy, and execution behavior.

    Host-owned named operation
       | action / parameter vocabulary
       v
    CommandProposalBuilder
       | generic action + typed arguments
       v
    Immutable CommandProposal
       |
       v
    CommandAiTextCodec (CommandAi/1)

## Example

~~~csharp
using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(200, 42)
        .WithText(201, "diagnostic stream"))
    .Build();

var wire = CommandAiTextCodec.Encode(proposal);
var received = CommandAiTextCodec.Decode(wire);
~~~

The numeric identities are illustrative only. Their meaning must come from a trusted host-owned vocabulary. Literal text is data, not executable code or trusted instruction.

## Extension boundary

A host may add named methods such as `InspectResource` or `OpenDiagnosticStream`. Such methods translate domain concepts into the generic action and parameter identities. They should validate their own inputs before constructing the proposal and should retain the authority to reject it later.

Construction and encoding do not authorize execution. The host resolves identities, validates current state, applies policy and approval requirements, dispatches the action, and records actual outcomes.

## Current ownership

| Capability | Owner |
|---|---|
| Immutable proposal, action IDs, typed arguments | CommandAi |
| CommandAi/1 codec and representation bounds | CommandAi |
| Named operations and identity mapping | Consuming domain / host |
| Vocabulary semantics | ProtocolAi / application domain |
| Grammar definitions, if used | GrammarAi |
| Endpoint request/response transport | HeadlessAi |
| CI evidence and verdicts | CiAi |
| Authorization, approval, dispatch, execution | FSM_Foundry / host |

Do not add CommandAi/2-only fields or version bumps to accommodate an internal consumer. Internal Workshop consumers should adapt to the approved /1 contract.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal — UML neighborhood and use cases

> **Target:** public sealed record CommandProposal. Relationships are authored from current source; they are not runtime traces or generated UML.

## Neighborhood

~~~mermaid
classDiagram
  class CommandProposal {
    +string ProtocolVersion
    +IReadOnlyList~CommandInstruction~ Commands
    +CurrentProtocolVersion = CommandAi/2
  }
  CommandProposal --> CommandInstruction : ordered atomic commands
  CommandInstruction --> CommandValue : target / scope
  CommandInstruction --> CommandArgument : parameters
  CommandInstruction --> CommandPrecondition : grammar-versioned conditions
  CommandInstruction --> CommandOutcomeContract : expected outcome shape
~~~

## Source-supported relationships

- **Target source:** [CommandProposal.cs](../../CommandProposal.cs).
- **Related source:** CommandInstruction.cs and CommandLimits.cs.
- **Codec:** CommandAiTextCodec.cs serializes each command entry as JSON within the CommandAi/2 envelope.
- **Invariants:** proposals are non-empty, ordered, bounded, and unique by command identity and idempotency key.

## Use cases

### UC-1 — Consume a proposed batch safely
- **Actor:** host application.
- **Trigger:** the host receives a candidate CommandAi/2 envelope.
- **Outcome:** a parsed proposal is available for host-owned validation; no side effect occurs in CommandAi.
- **Failure:** malformed content and unsupported versions are rejected.

### UC-2 — Coordinate atomic commands
- **Actor:** FSM_Foundry or another host.
- **Goal:** inspect the ordered proposal, then independently validate and authorize each atomic command.
- **Outcome:** the host may accept, reject, retry, or dispatch commands according to its policy. The proposal itself does not imply transaction semantics or all-or-nothing execution.

## Related pages

- [Type page](../types/CommandProposal.md)
- [Problem domain](../PROBLEM_DOMAIN.md)
- [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

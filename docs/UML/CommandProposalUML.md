<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposal — UML neighborhood and use cases

> Relationships are authored from the approved source model; this is not a runtime trace or generated UML.

## Neighborhood

~~~mermaid
classDiagram
  class CommandProposal {
    +string ProtocolVersion
    +IReadOnlyList~CommandInstruction~ Commands
    +CurrentProtocolVersion = CommandAi/1
  }
  class CommandInstruction {
    +Guid? CommandId
    +ulong ActionId
    +CommandValue? Target
    +CommandValue? Scope
    +string? IdempotencyKey
    +IReadOnlyList~CommandArgument~ Arguments
    +IReadOnlyList~CommandPrecondition~ Preconditions
    +CommandOutcomeContract? OutcomeContract
  }
  class CommandPrecondition {
    +string GrammarVersion
    +string Expression
  }
  class CommandOutcomeContract {
    +ulong OutcomeId
    +IReadOnlyList~CommandOutcomeField~ Fields
  }
  CommandProposal --> CommandInstruction : ordered compact/enriched commands
  CommandInstruction --> CommandPrecondition : opaque conditions
  CommandInstruction --> CommandOutcomeContract : expected shape
~~~

## Source-supported relationships

- Target source: [CommandProposal.cs](../../CommandProposal.cs).
- Related source: CommandInstruction.cs, CommandArgument.cs, CommandValue.cs, CommandPrecondition.cs, CommandOutcomeContract.cs, CommandLimits.cs.
- Codec: CommandAiTextCodec.cs reads and writes compact and enriched CommandAi/1 entries.
- Invariants: proposals are non-empty, ordered, bounded; enriched command IDs and idempotency identities are unique; legacy compact instructions do not receive invented metadata.

## Use cases

### UC-1 — Decode a proposed batch
- Actor: consuming host.
- Trigger: receive a candidate CommandAi/1 envelope.
- Outcome: a structurally valid proposal is available for host-owned semantic validation.
- Failure: malformed content, unsupported protocol, invalid identities, duplicates, or resource-limit violations are rejected.

### UC-2 — Compose atomic actions
- Actor: application or host.
- Goal: represent several ordered action proposals.
- Outcome: the host receives an ordered list. The list does not imply transaction, dependency, retry, replay, or rollback semantics.

## Related pages

- [Type page](../types/CommandProposal.md)
- [Theory](../THEORY.md)
- [Problem domain](../PROBLEM_DOMAIN.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

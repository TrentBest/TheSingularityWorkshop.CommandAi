<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstruction — UML neighborhood and use cases

> **Target:** public sealed record CommandInstruction. Relationships are authored from current source; they are not runtime traces or generated UML.

## Neighborhood

~~~mermaid
classDiagram
  class CommandInstruction {
    +Guid CommandId
    +ulong ActionId
    +CommandValue Target
    +CommandValue Scope
    +string IdempotencyKey
    +IReadOnlyList~CommandArgument~ Arguments
    +IReadOnlyList~CommandPrecondition~ Preconditions
    +CommandOutcomeContract OutcomeContract
  }
  CommandInstruction --> CommandValue : target / scope
  CommandInstruction --> CommandArgument : parameters
  CommandInstruction --> CommandPrecondition : opaque conditions
  CommandInstruction --> CommandOutcomeContract : expected result shape
  CommandProposal --> CommandInstruction : ordered batch
~~~

## Source-supported relationships

- **Target source:** [CommandInstruction.cs](../../CommandInstruction.cs).
- **Related source:** CommandValue.cs, CommandArgument.cs, CommandPrecondition.cs, CommandOutcomeContract.cs, CommandProposal.cs.
- **Codec:** CommandAiTextCodec.cs encodes/decodes the complete contract.
- **Package boundary:** CommandAi stores precondition expressions without interpreting them and stores outcome declarations without treating them as actual results.

## Use cases

### UC-1 — Inspect an untrusted proposal
- **Actor:** host application.
- **Trigger:** a candidate CommandAi/2 envelope arrives.
- **Outcome:** the codec returns a bounded immutable representation for host validation.
- **Failure:** malformed input or unsupported versions are rejected.
- **Safety invariant:** parsing does not authorize or execute.

### UC-2 — Evaluate and dispatch through the host
- **Actor:** FSM_Foundry or another trusted host.
- **Preconditions:** the host resolves action/target/scope identities, validates parameters, interprets preconditions with the appropriate grammar, enforces idempotency, and applies authorization/approval.
- **Outcome:** the host may dispatch the accepted command and later record an actual correlated outcome.
- **Ownership:** actual outcomes belong to ProtocolAi/host; CI verdicts belong to CiAi.

## Detail levels

- **Collapsed:** CommandInstruction and its role as one atomic proposal.
- **Contract:** public members are defined by the linked source file.
- **Implementation:** see source links; this static page is not interactive generated UML.

## Related pages

- [Type page](../types/CommandInstruction.md)
- [Problem domain](../PROBLEM_DOMAIN.md)
- [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

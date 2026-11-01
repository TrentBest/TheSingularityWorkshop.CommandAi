<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# The Workshop AI package series

## One stack, separate responsibilities

| Package | Domain question | Responsibility | Explicit non-goal |
|---|---|---|---|
| [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | What does this identity mean? | Application-owned vocabulary, stable protocol-qualified symbol identity, correlated/provenance-bearing events and actual outcomes | Inference, authorization, execution |
| [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Which symbol structures are valid? | Grammar definitions, productions, and structural validation | Owning vocabulary meaning, live-state truth, execution |
| **CommandAi** | What complete atomic command is being proposed? | Versioned command identity, action, target, scope, parameters, preconditions, idempotency, and expected outcome contract | Evaluating conditions, enforcing idempotency, granting authority, or dispatching commands |
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | How does a host exchange content with an endpoint? | Configured AI endpoint request/response communication | Owning task policy, memory/state, command meaning, or execution |
| [CiAi](https://github.com/TrentBest/TheSingularityWorkshop.CiAi) | What CI evidence and verdict apply? | CI evidence contracts, evidence validation, and verdicts | General command authority or execution |

The host application and FSM_Foundry surround these packages with trusted configuration, domain validation, authorization, approval policy, durable observer lifecycle, scheduling, dispatch, execution, and outcome lifecycle. A response from an AI endpoint is untrusted input even when it parses correctly.

## Conceptual data flow

    Host defines trusted vocabulary and grammar
                     |
                     v
    HeadlessAi sends request to configured endpoint
                     |
                     v
    Model returns candidate content
                     |
                     v
    Host/adapter validates expected structure
                     |
                     v
    CommandAi represents complete proposed commands
                     |
                     v
    ProtocolAi resolves vocabulary and carries correlated outcomes
                     |
                     v
    GrammarAi validates structural composition
                     |
                     v
    Host/FSM_Foundry evaluates live state, authorizes, approves,
    schedules, dispatches, and executes accepted work
                     |
                     v
    ProtocolAi records actual correlated outcomes;
    CiAi evaluates CI evidence and verdicts where applicable

This is an architectural flow, not a claim that every package is already wired into one turnkey integration. The host may order structural validation and identity resolution according to its domain. The invariant is that parsing, lookup, grammar validity, and declared outcome shape never authorize execution.

## CommandAi/2 contract

CommandAi/2 carries a unique command identity, action identity, target, scope, ordered parameters, optional grammar-versioned preconditions, required idempotency key, and expected outcome contract. It uses opaque identifiers and has no runtime dependency on ProtocolAi, GrammarAi, HeadlessAi, or CiAi.

CommandAi/1 is intentionally not supported. The expanded contract is a deliberate breaking change and the package has not been externally released.

## Security invariant

**Parsing is not permission.** A well-formed proposal can still be malicious, incorrect, stale, or unauthorized. A host must resolve identities against trusted definitions, evaluate preconditions against live state, enforce idempotency, apply authorization and approval, and decide whether to execute.

## Reading path

- [CommandAi problem domain](PROBLEM_DOMAIN.md)
- [CommandAi architecture](ARCHITECTURE.md)
- [CommandAi wire contract](THEORY.md)
- [CommandAi usage](USAGE.md)
- [CommandAi status](REFLECTION.md)
- [Shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

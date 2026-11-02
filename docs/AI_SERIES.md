<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# The Workshop AI package series

## Separate responsibilities

| Package | Domain question | Responsibility | Explicit non-goal |
|---|---|---|---|
| ProtocolAi | What does this identity mean? | Protocol vocabulary, stable identity meaning, correlated events and actual outcomes | Command representation, authorization, execution |
| GrammarAi | Which structures are valid? | Grammar definitions, productions, structural validation | Vocabulary meaning, live-state truth, execution |
| CommandAi | What action is being proposed? | Immutable ordered action/argument proposal, bounds, builders, CommandAi/1 codec | Workflow, authority, authorization, execution |
| HeadlessAi | How does a host communicate with an endpoint? | Configured AI endpoint request/response transport | Command authority or execution |
| CiAi | What CI evidence and verdict apply? | CI evidence and verdicts | General command authorization or execution |

FSM_Foundry and the consuming host own trusted configuration, domain validation, live-state checks, authorization, approval, scheduling, dispatch, execution, and actual outcome lifecycle.

## Conceptual flow

    Host defines trusted vocabulary and policy
                    |
                    v
    HeadlessAi exchanges content with endpoint
                    |
                    v
    Candidate response is treated as untrusted
                    |
                    v
    CommandAi represents ordered proposed actions
                    |
                    v
    Host resolves IDs and validates domain state
                    |
                    v
    Foundry / host authorizes and executes accepted work
                    |
                    v
    ProtocolAi / runtime records actual outcomes
                    |
                    v
    CiAi evaluates CI evidence where applicable

This is an ownership map, not a claim that every package is wired into one turnkey pipeline. Parsing and grammar validation never authorize execution.

## CommandAi/1

CommandAi/1 is the approved contract. It contains a proposal with ordered commands, positive action IDs, and ordered typed arguments. Values are positive integer identifiers or JSON-quoted literal strings. The package intentionally does not carry target/scope, idempotency, precondition, or expected-outcome fields.

Internal Workshop consumers must adapt to /1. Do not introduce /2-only fields or version bumps merely for internal coordination.

## Security invariant

**Parsing is not permission.** A well-formed proposal can still be malicious, incorrect, stale, or unauthorized. The host must resolve identifiers against trusted definitions and apply its own validation and policy before any side effect.

## Reading path

- [Problem domain](PROBLEM_DOMAIN.md)
- [Architecture](ARCHITECTURE.md)
- [Theory and wire contract](THEORY.md)
- [Usage](USAGE.md)
- [Status](REFLECTION.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

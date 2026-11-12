<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# The Workshop AI package series

## Separate responsibilities

| Package | Domain question | Responsibility | Explicit non-goal |
|---|---|---|---|
| ProtocolAi | What does this identity/message mean? | Vocabulary identity, correlated events and actual outcome envelopes | Command representation, authorization, execution |
| GrammarAi | Which structures are valid? | Grammar definitions and structural validation of grammar-versioned preconditions | Vocabulary meaning, live-state truth, authorization, execution |
| CommandAi | What atomic action is proposed? | Immutable command/action/target/scope/precondition/idempotency/outcome declarations and typed arguments | Grammar evaluation, replay enforcement, workflow, authority, execution |
| HeadlessAi | How does a host communicate with an endpoint? | Configured AI endpoint request/response transport | Command authority or execution |
| CiAi | What CI evidence and verdict apply? | CI evidence and verdicts, plus domain-specific proposal composition | General command authorization or execution |

FSM_Foundry and the consuming host own trusted configuration, live-state evaluation, policy, authorization, approval, replay/delivery, scheduling, dispatch, execution, and actual outcome lifecycle.

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
    CommandAi represents proposed atomic commands
                    |
                    v
    GrammarAi validates precondition structure
                    |
                    v
    Host evaluates live state and validates domain scope
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

## CommandAi/1 proposal forms

The original compact Command= entry remains supported with byte-for-byte stable encoding. The enriched atomic-command form adds an explicit AtomicCommand= JSON entry under the same CommandAi/1 protocol identifier. New readers accept compact, enriched, and mixed proposals.

The enriched form carries command identity distinct from action identity, typed target and scope, bounded idempotency identity, expected-outcome shape, ordered typed arguments, and optional grammar-versioned preconditions. Compact entries retain enriched metadata as absent; no defaults are inferred.

**Compatibility is directional:** older compact-only readers reject the new AtomicCommand= entry and must be upgraded before consuming enriched proposals. The unchanged /1 label does not make old readers forward-compatible.

CommandAi preserves precondition data; GrammarAi owns grammar and structural validation; the host evaluates live state. CommandAi carries an idempotency identity; the host implements replay/deduplication. An expected-outcome contract is a declaration, not an actual outcome.

## Security invariant

**Parsing is not permission.** A well-formed proposal can still be malicious, incorrect, stale, or unauthorized. The host must resolve identifiers against trusted definitions and apply its own validation and policy before any side effect.

## Reading path

- [Problem domain](PROBLEM_DOMAIN.md)
- [Architecture](ARCHITECTURE.md)
- [Theory and wire contract](THEORY.md)
- [Usage](USAGE.md)
- [Builder integration](BUILDER_INTEGRATION.md)
- [Status](REFLECTION.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

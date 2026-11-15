<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi domain completeness and ownership

> Status: this document describes the compact CommandAi/1 proposal model currently present on `development`. Richer work in draft PR #8 is tracked separately and is not part of the current development package.

## 01 — What does CommandAi own?

CommandAi represents a bounded, provider-neutral proposal of ordered atomic actions. Each instruction has a positive opaque action identity and an ordered collection of positive parameter identities paired with either a positive integer identity or literal text.

The current public model is centered on:

- `CommandProposal` — immutable, non-empty ordered proposal.
- `CommandInstruction` — one atomic action declaration.
- `CommandArgument` — one parameter identity/value pair.
- `CommandValue` and `CommandValueKind` — integer-identity or literal-text representation.
- `CommandLimits` — structural resource bounds.
- `CommandAiTextCodec` — CommandAi/1 encoding and decoding.
- `CommandProposalBuilder` and `CommandInstructionBuilder` — mutable authoring helpers that produce immutable snapshots.

## 02 — Why is the boundary deliberately narrow?

A model can describe an action without knowing what the action means or whether it is allowed. A host needs a predictable representation but must retain trusted vocabulary mappings, current-state checks, policy, authorization, approval, dispatch, execution, and actual outcome recording.

CommandAi therefore owns representation and structural validation, not application semantics or authority. Positive integer identities are opaque identifiers, not pointers, capabilities, or authorization tokens. A successfully decoded proposal remains untrusted intent.

## 03 — How are responsibilities divided?

| Concern | Owner |
|---|---|
| Shared vocabulary identity and broader causal/event/outcome provenance | ProtocolAi |
| Grammar definitions and grammar-specific structural validation | GrammarAi |
| Immutable proposal model, builders, bounds, CommandAi/1 codec | CommandAi core |
| Endpoint communication | HeadlessAi |
| CI evidence and verdicts | CiAi |
| Trusted mappings, domain validation, live-state checks, policy, authorization, approval, dispatch, execution | FSM_Foundry / consuming host |
| MicroBundle identity, dependencies, load lifecycle, composition participation | MicroBundleDomain |
| Artifact discovery, storage, retrieval, verification, materialization | MicroBundleRepository |
| Dependency resolution and composition orchestration | FSM_COS or another composition host |

CommandAi has no runtime dependency on GrammarAi, ProtocolAi, HeadlessAi, FSM_COS, or MicroBundleRepository.

### Optional MicroBundle distribution

The optional `TheSingularityWorkshop.CommandAi.MicroBundle` package implements `IMicroBundle` from `TheSingularityWorkshop.MicroBundleDomain` 1.0.1. It declares provider `commandai.proposal` and delegates Encode/Decode to the core codec.

The package split is intentional:

- `TheSingularityWorkshop.CommandAi` remains dependency-free for ordinary library consumers.
- `TheSingularityWorkshop.CommandAi.MicroBundle` depends on the core package and the neutral MicroBundleDomain contract.
- Neither package depends on FSM_COS or MicroBundleRepository.
- The adapter's Load lifecycle is stateless; its Arbitrate method reports no composition mutation. Neither method executes a proposal.

## 04 — What is implemented and verified?

The current compact CommandAi/1 codec validates the envelope, protocol identity, command/argument identities, typed values, and configured resource limits. The encoder emits deterministic LF separators; the decoder accepts LF and CRLF.

Exact-head [CommandAi CI run 43683852447](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43683852447) passed on `d3b26f14490eac18966e813e4ceb66e4d3c996db`. It completed 58 tests, built and ran the core quickstart, packed and audited the dependency-free core package, packed the optional adapter against that core package, checked adapter package metadata/dependency closure, and restored, built, and ran a package-only adapter consumer. NuGet publication was skipped.

The package identities verified by that run were:

- Core: `TheSingularityWorkshop.CommandAi` `0.1.0-alpha.2`.
- Adapter: `TheSingularityWorkshop.CommandAi.MicroBundle` `0.1.0-alpha.1`.
- MicroBundle contract: `TheSingularityWorkshop.MicroBundleDomain` `1.0.1`.

## 05 — What is not part of the current development package?

The following must not be described as current `development` behavior until the candidate source is reconciled and verified there:

- Address-space `CommandExchange` and `CommandResponse` models and their JSON codec.
- Enriched `AtomicCommand` entries, preconditions, idempotency declarations, and expected-outcome contracts.
- The optional opaque GrammarAi command-body transport seam.

Those features are documented and tested on draft PR #8, whose exact-head evidence is recorded in that PR. They are not silently included in the current development package or the package artifacts verified above. Decide whether to incorporate that candidate before a public release, then rerun source, documentation, compatibility, package, and consumer verification on the resulting exact head.

A proposal is not a transaction, workflow, authorization token, execution receipt, or actual outcome. Ordering is preserved, but atomicity, rollback, retry, cancellation, replay enforcement, idempotency enforcement, and execution guarantees remain outside the current compact proposal contract.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
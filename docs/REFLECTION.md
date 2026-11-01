<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and open questions

## Current implementation

- Immutable proposal, instruction, argument, and typed-value contracts.
- Generic fluent CommandProposalBuilder and scoped CommandInstructionBuilder; application-specific named commands remain host/domain extensions.
- Explicit command, argument, per-literal, aggregate-literal, and encoded-envelope resource bounds.
- Line-oriented CommandAi/1 encoder/decoder with explicit JSON-quoted literal values.
- Positive-integer validation, duplicate-parameter rejection, strict envelope-entry handling, and normalized FormatException failures for malformed non-empty wire content.
- Regression tests for empty proposals, duplicate parameter identities, invalid literals, resource limits, round trips, builder behavior, and immutable snapshots.
- Console quickstart with compiled host-owned named-command extensions.
- No runtime package references to the other AI packages.
- No command execution, approval UI, authorization, provider transport, durable observer, or outcome lifecycle.

## Known limits and explicit non-goals

- Identifier meaning is host-defined and not resolved here.
- The initial wire format is intentionally narrow and does not support nested objects or lists as values.
- The only implemented envelope is CommandAi/1; version negotiation and migration policy are not implemented.
- An instruction is an atomic proposed action; a proposal is an ordered batch, not a transaction. Target, scope, precondition, idempotency, and outcome semantics are not universal CommandAi concepts in this version. Hosts may encode domain-owned values as arguments, while durable observer/evidence/gate lifecycles belong to host and sibling protocol contracts.
- CommandAi does not poll, subscribe to events, evaluate CI evidence, change gates, or dispatch commands. A decoded or well-formed proposal is never authorization.
- The last previously recorded full verification is GitHub Actions run [#92](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41283963463): build/test, quickstart execution, package packing, packed-artifact audit, and compilation against the packed package passed; NuGet publication was skipped. New decoder hardening and tests are not yet CI-verified.

## Next evidence to gather

1. Run the complete workflow against the latest development commit and inspect the packed package/consumer compilation after the decoder changes.
2. Add a host-owned integration example only when the current ProtocolAi and GrammarAi public contracts can be composed without introducing unnecessary generic-package coupling.
3. Keep authorization, approval, event subscriptions, durable observer state, evidence evaluation, and execution in their owning host/protocol domains; document any cross-repository contract through linked issues before claiming end-to-end behavior.

Legacy compatibility is intentionally not planned. The source-level observations are retained in Legacy CommandAi — historical reference; no compatibility parser or fixture corpus should be built.

No NuGet publication is authorized by this status page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

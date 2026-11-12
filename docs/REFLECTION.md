<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and release-readiness evidence

## Current candidate

- Pull request: [#8 — enriched atomic CommandAi/1 integration](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/pull/8)
- Candidate branch: `work/commandai-enriched-v1-integration`
- Current PR head: `eb0d6533149c5690518fdded97881d062da6f7b5`
- The preceding documented exact-head CI run [43163337018](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43163337018) passed on `a10722ae3013f2fa0526c4d275f77dd0bf9a13b8`: `build-and-test` succeeded and `publish_nuget` was skipped.
- Subsequent commits add the ordered-address-space response contract documentation and regression test, followed by focused atomic-model, builder, and codec guard tests (`d159b521` and `0b8f29c`). The latest exact-head workflow lookup returned no PR workflow run for test head `0b8f29cf4b36a4bb525c98218f1de79d86b43e1f`. This documentation refresh advances the branch to `eb0d6533149c5690518fdded97881d062da6f7b5`; neither the test head nor this latest documentation head is yet CI-verified.
- The latest Codecov comment reports **81.23077% patch coverage** with 61 changed lines missing coverage, but it predates the latest test commits. Treat it as historical only; refresh coverage against the current head before drawing conclusions.

Every exact-head verification claim applies only to the SHA named beside its run. A later source or documentation commit requires its own evidence.

## Approved design direction and compatibility

The owner clarification recorded on 2026-11-04 supersedes the earlier blanket instruction to keep /1 unchanged: relevant data may be carried in the existing CommandAi/1 contract when it adds value without payload bloat; if it materially bloats the base contract, use a deliberate extension/version rather than forcing it into the base shape.

The candidate implements enriched commands as explicit `AtomicCommand=` JSON entries while preserving the original compact `Command=` representation. New readers accept compact, enriched, and mixed proposals. Compact encoding remains byte-for-byte stable and compact instructions do not receive inferred enriched defaults.

**Compatibility limitation:** older compact-only readers reject `AtomicCommand=` entries. They must be upgraded before consuming enriched proposals. The unchanged /1 protocol identifier is not a promise that old readers understand the enriched entry.

A representative sample measured 45 characters for a compact command and 740 characters for the enriched JSON line (about 16.4×). This is not a like-for-like semantic comparison: the enriched record carries target, scope, command identity, idempotency identity, expected-outcome shape, and typed data that the compact sample omits. Keep the enriched entry explicit rather than inflating every compact command, and continue to weigh the payload cost against consumer value.

## Contract and ownership

- CommandAi owns immutable command/proposal models, builders, representation limits, and compact/enriched codecs.
- GrammarAi owns precondition grammar and structural validation; CommandAi preserves grammar version and expression without interpreting them.
- ProtocolAi/runtime owns correlated actual event/outcome envelopes.
- The consuming host / FSM_Foundry owns trusted identity resolution, live-state evaluation, replay/deduplication enforcement, authorization, approval, scheduling, dispatch, and execution.
- `CommandOutcomeContract` declares expected shape only. It is not an actual result, success signal, CI verdict, or proof of execution.
- CommandAi remains independent of UI and sibling runtime packages. Profiles is not a required dependency.
- Address-space descriptor order is part of the exchange contract. A response must preserve the exact descriptor sequence from its originating request; reordered-but-equivalent descriptors are rejected by `ValidateFor`.

## Focused verification

The suite covers compact byte-for-byte compatibility, mixed compact/enriched ordering, unknown/duplicate JSON properties, required fields, the required outcome-field flag, typed targets/scopes, identity uniqueness, and bounded collections. Follow-up tests cover null nested JSON entries, invalid typed-value combinations, exact collection boundaries, and rejection of reordered response descriptors. The latest test-only commits add constructor/builder guard cases, duplicate and capacity checks, malformed-envelope handling, compact argument overflow, non-object JSON, and invalid atomic model cases; these additions still require exact-head CI and refreshed coverage.

The exchange contract is structural only. Address IDs are identifiers, not pointers or memory capabilities. A well-formed response remains untrusted intent.

## Repository and release safety

- PR #8 remains open and draft, unmerged.
- PR #6 remains open and unmerged on `work/enriched-atomic-command-v1`; it has not been marked safe to delete. Its distinct history and any unique remaining documentation must be reconciled before branch cleanup.
- The long-term branch target remains exactly `master` and `development`; no work branch is ready to delete based on this audit.
- Package version remains `0.1.0-alpha.2`.
- NuGet publication is not authorized. No package was published.
- No merge, write to `master`, release, deployment, or branch deletion is authorized or performed by this work.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

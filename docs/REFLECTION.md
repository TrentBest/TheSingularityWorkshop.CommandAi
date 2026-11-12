<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and release-readiness evidence

## Current candidate

- Pull request: [#8 — enriched atomic CommandAi/1 integration](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/pull/8)
- Candidate branch: work/commandai-enriched-v1-integration
- Previously verified source head: a267e0d2e49b736cfebff106a27dfbabb4498f43
- Exact-head [CommandAi CI run 42765950143](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/42765950143) passed on that SHA.
- Workflow coverage included branding, restore/build, tests with coverage, Codecov upload, coverage artifact, quickstart, package pack, packed-artifact audit, and consumer compilation against the packed package. The publish_nuget job was skipped.
- Patch coverage reported by Codecov was **78.77%**; 69 changed lines were not fully covered. This is a follow-up test-quality signal, not a failed build/test gate. Partial-coverage areas include codec parsing, instruction/model validation, builder validation, and outcome-contract validation.

This document is a status snapshot. Its exact-head verification claim applies only to a267e0d; every subsequent source or documentation commit requires its own CI result.

## Approved design direction and compatibility

The owner clarification recorded on 2026-11-04 supersedes the earlier blanket instruction to keep /1 unchanged: relevant data may be carried in the existing CommandAi/1 contract when it adds value without payload bloat; if it materially bloats the base contract, use a deliberate extension/version rather than forcing it into the base shape.

The current candidate implements enriched commands as an explicit AtomicCommand= JSON entry while preserving the original compact Command= representation. New readers accept both forms and mixed proposals. Compact encoding remains byte-for-byte stable and compact instructions do not receive inferred enriched defaults.

**Compatibility limitation:** older compact-only readers reject AtomicCommand= entries. They must be upgraded before consuming enriched proposals. The unchanged /1 protocol identifier is not a promise that old readers understand the extension.

A representative sample measured 45 characters for a compact command and 740 characters for the enriched JSON line (about 16.4×). This is not a like-for-like semantic comparison: the enriched record carries target, scope, command identity, idempotency identity, expected-outcome shape, and typed data that the compact sample omits. The size difference is material and should be weighed against consumer value; it is not, by itself, proof that the design should be rejected. Keep the enriched entry explicit rather than inflating every compact command.

## Contract and ownership

- CommandAi owns immutable command/proposal models, builders, representation limits, and the compact/enriched codec.
- GrammarAi owns precondition grammar and structural validation; CommandAi preserves grammar version and expression without interpreting them.
- ProtocolAi/runtime owns correlated actual event/outcome envelopes.
- The consuming host / FSM_Foundry owns trusted identity resolution, live-state evaluation, replay/deduplication enforcement, authorization, approval, scheduling, dispatch, and execution.
- CommandOutcomeContract declares an expected shape only. It is not an actual result, success signal, CI verdict, or proof of execution.
- CommandAi remains independent of UI and sibling runtime packages. Profiles is not a required dependency.

## Focused follow-up

The current test suite already covers compact byte-for-byte compatibility, mixed compact/enriched ordering, unknown/duplicate JSON properties, required top-level fields, the required outcome-field flag, typed targets/scopes, identity uniqueness, and key resource bounds.

The follow-up commit adds focused regression coverage for null nested JSON entries, invalid typed-value combinations, and exact supported collection boundaries. These cases exercise defensive checks already present in the parser/model rather than changing wire semantics.

## Release and repository safety

- Package version remains 0.1.0-alpha.2.
- NuGet publication is not authorized and remains skipped.
- PR #8 remains draft pending review of payload trade-offs, compatibility, and exact-head CI after this follow-up.
- No merge, master write, release, deployment, or branch deletion is part of this work.
- PR #6 remains a documentation source for remaining UML/type-guide reconciliation. Do not delete its work branch until all unique useful material has been audited and ported.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

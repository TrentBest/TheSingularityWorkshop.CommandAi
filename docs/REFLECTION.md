<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and release-readiness evidence

## Current candidate

- **Candidate review snapshot:** the approved optional opaque GrammarAi body transport and expanded domain-completeness ownership matrix are included on this PR branch. The PR description records the current exact branch head; exact-head workflow/status checks must be repeated after every subsequent commit.
- **Latest verified ancestor:** `5fbf4bf7303966b6413fc2ba70d7d5aaf74c14f5`, verified by [CommandAi CI run 43508426972](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43508426972). That successful run is evidence for that SHA only, not for later commits.

- Pull request: [#8 — enriched atomic CommandAi/1 integration](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/pull/8)
- Candidate branch: `work/commandai-enriched-v1-integration`
- Latest verified ancestor: `5fbf4bf7303966b6413fc2ba70d7d5aaf74c14f5` (see exact-head evidence above); subsequent candidate commits remain unverified until CI runs on their exact SHA.
- Exact-head CI: [CommandAi CI run 43508426972](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/43508426972) ran on this exact SHA and completed successfully. The `build-and-test` job succeeded; `publish_nuget` was skipped.
- The successful run covered branding verification, restore, build, tests with coverage, Codecov upload, coverage artifact upload, quickstart validation, package packing, audit/compile of the quickstart against the packed package, and package artifact upload. This is validation evidence only; no package was published.
- The run's Cobertura artifact reports **96.77% overall line coverage (751/776)** and **85.30% branch coverage (418/490)**. These are repository-wide totals, not patch coverage. The artifact also reports 100% line coverage for `CommandGrammarBody.cs` and `CommandInstructionBuilder.cs`, 98.20% for `CommandAiTextCodec.cs`, 91.11% for `CommandExchange.cs`, 90.80% for `CommandExchangeTextCodec.cs`, and 96.97% for `CommandProposal.cs`.
- The exact-head workflow evidence supersedes earlier notes that the later atomic guard tests had no verification. The run is tied to the SHA above; any subsequent source or documentation commit requires its own exact-head verification.
- PR #8 remains open and draft, targeting `development`. At the latest branch comparison, it is 42 commits ahead and 5 behind `development`; synchronize deliberately and rerun exact-head CI after any update.

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

The suite covers compact byte-for-byte compatibility, mixed compact/enriched ordering, unknown/duplicate JSON properties, required fields, the required outcome-field flag, typed targets/scopes, identity uniqueness, and bounded collections. Follow-up tests cover null nested JSON entries, invalid typed-value combinations, exact collection boundaries, and rejection of reordered response descriptors. The latest focused guard tests cover constructor/builder preconditions, duplicate and capacity checks, malformed-envelope handling, compact argument overflow, non-object JSON, and invalid atomic model cases. These tests are included in the exact-head CI run cited above.

The exchange contract is structural only. Address IDs are identifiers, not pointers or memory capabilities. A well-formed response remains untrusted intent.

## Reconciliation and downstream integration gates

- PR #6 remains open and unmerged on `work/enriched-atomic-command-v1`. Its history is divergent from PR #8; do not merge it wholesale. A content-level documentation audit is in progress. Sampled architecture, README, UML/type guidance, and agent instructions have been compared, but the full document-by-document audit is not yet complete. PR #6 is not declared safe to delete.
- GrammarAi [issue #14](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi/issues/14) has its grammar-owned acceptance criteria complete, but its real consumer/host-adapter proof remains unchecked. The required proof must use the exact pinned GrammarAi revision and real ProtocolAi/CommandAi/host adapters, and distinguish True, False, Unknown, Incomplete, failed events, and mismatched scope. CommandAi's own opaque-preservation tests do not satisfy this downstream proof.
- PR #8 and PR #6 histories are substantially divergent. Reconcile unique content and plan synchronization with `development` explicitly; do not use a blind merge/rebase as a substitute for the audit.

## Repository and release safety

- The long-term branch target remains exactly `master` and `development`. The current repository has those two canonical branches plus the two active PR work branches; neither work branch is ready to delete based on this audit.
- Package version remains `0.1.0-alpha.2`.
- NuGet publication is not authorized. No package was published.
- No merge, write to `master`, release, deployment, or branch deletion is authorized or performed by this work.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

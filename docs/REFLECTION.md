<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and release-readiness evidence

## Approved direction

**Retain CommandAi/1.** The Workshop owns all current consumers, so internal repositories adapt together to the existing /1 model and wire shape. Do not migrate to /2, add /2-only fields, or bump the package/protocol version merely for internal coordination.

Package version remains `0.1.0-alpha.2`. NuGet publication is not authorized.

## Current implementation

- Immutable `CommandProposal` and `CommandInstruction` model.
- Positive integer action and parameter identities.
- Ordered typed arguments using integer identifiers or literal text.
- Generic fluent proposal and instruction builders.
- Line-oriented CommandAi/1 codec.
- Bounds for envelope, command count, arguments, individual literal text, and aggregate literal text.
- Decoder rejects unsupported versions, malformed envelopes, duplicate protocol declarations, duplicate parameters, invalid values, malformed quoted literals, and resource-limit violations.
- Host owns domain meaning, authorization, approval, execution, and actual outcomes.

## Verification status

- **Latest development head passed:** [run 41711472222](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41711472222) completed successfully for exact head `3563bfcabdae9e4166c6b9f86a65edf959ab5631` on 2026-11-03. The `build-and-test` check succeeded; `publish_nuget` was skipped.
- The immediately preceding source-fix head `14a0828fbce994eddef9f9d229afbbb8e3b16e8a` also passed [run 41594167943](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41594167943).
- These runs verify the null-command validation-order fix and its regression test. A null command in the proposal input now produces the intended `ArgumentException` (with parameter name `commands`) rather than an accidental `NullReferenceException` during aggregate literal-size inspection.
- The earlier full workflow passed on exact development head `339f404b0c7ad0170ca1549da1161938047c1ca8`: [run 41592630562](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41592630562). It completed Workshop Markdown branding validation, .NET restore/build, tests with coverage, Codecov upload, quickstart execution, package pack, packed-artifact audit, and consumer compilation against the packed package.
- The `publish_nuget` job was skipped. The package artifact was produced but not published.
- The verified implementation includes adversarial codec tests and deterministic-LF encoding. The encoder emits LF line separators on every platform; the decoder accepts LF and CRLF.
- The approved package version remains `0.1.0-alpha.2`. No package/protocol version increment or NuGet publication occurred.

## Latest development activity (2026-11-03)

- Current `development` head is `3563bfcabdae9e4166c6b9f86a65edf959ab5631` (`ci: retain verified Codecov token upload`). Its parent is `a2edf58e5500a65507c95fa2ffe48a9734aec29f` (`ci: add CiAi commit evidence observer`). Exact-head workflow run 41711472222 passed `build-and-test`; `publish_nuget` was skipped.
- The CiAi observer workflow is now present on `development`, but it is **not activated** by GitHub's `workflow_run` trigger until that workflow is present on the default branch. The owner-approved activation boundary remains unchanged: no `master` modification or merge is authorized.
- PR #5 remains open/draft against `master`; its head `d782ed6c1979e57ab5d6443f07614990e4ad3194` is 4 commits ahead and 260 behind `development`. Its diff includes the observer workflow plus duplicate root-level model/test files that already exist on `development`; do not merge this stale branch as-is. Reconcile or retire it only with explicit branch/PR disposition authorization.
- PR #4 is closed unmerged. Its five-commit branch remains 5 ahead and 57 behind `development`; the intended `/1` corrections are already represented in current development docs. The branch is not needed as a merge source; do not delete it without explicit authorization.

- Approved CommandAi/1 source and API remain the only supported contract.
- Adversarial codec and boundary tests were added in `b1f4abae7dc708f0345747c280eec22edc18e5f2`.
- The domain matrix was corrected in `084dbe7ebcc9a1f224a36c22c86a835e19a3a5d3` to distinguish /1 exclusions from implemented fields and to keep correlation/provenance and actual outcomes with ProtocolAi/runtime.
- Package metadata was corrected in `a92610be1b2c3d0b5b5f5d4cc5e6e0799e1ace14`; its description now accurately identifies the CommandAi/1 envelope.
- Deterministic LF text encoding was implemented in `1b7211d2638de680a75edddced78f6a7a60ef4e0`, with an exact-envelope regression test in `c59601eb940952c51e09344224d3c95d0f422c61`.
- A null-command validation-order defect was fixed in `14a0828fbce994eddef9f9d229afbbb8e3b16e8a`; regression coverage was added in `3af6d5754d10e75837531fef14d5f1de11973b7b`. Both exact-head CI runs passed (41594167943 and 41594181593).
- The complete Markdown inventory was audited: all 31 Markdown pages have the animated moniker and canonical Workshop PNG footer. Removed /2-only model type references are absent; remaining /2 mentions are historical/correction notes.
- CiAi PR #37 merged at `49a802fcab60fa7411b01892072d7e2c8a2c72a9`. Draft CiAi PR #50 advances its submodule pin to `339f404b0c7ad0170ca1549da1161938047c1ca8`; both required workflows passed on exact PR head `6499e3a301dfeeeeafaff793ee392120efadedae` (runs 41592774387 and 41592774359). PR #50 remains open/draft; no merge or publication was performed.
- Package version remains `0.1.0-alpha.2`; no package/protocol version increment or NuGet publication.

## Remaining work

1. Review optionality/default/schema and compatibility policy without inventing unsupported /1 semantics; add generic schema features only when a concrete consumer need and owner are established.
2. Coordinate cross-repository ownership for observer registration, event correlation/replay, and evidence-bound gate transitions through the relevant issues.
3. Resolve the package-distribution strategy for consumers of CiAi's transitive CommandAi dependency before any package release.
4. Keep CiAi PR #50 draft until the owner explicitly authorizes its merge; both required exact-head workflows have passed.

No user-held decision currently blocks the /1 source audit. Cross-repository runtime semantics remain agent-owned coordination work; they do not block the package's current /1 representation review.

No further merge, NuGet publication, release, deployment, master alteration, or branch deletion is authorized by this work.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

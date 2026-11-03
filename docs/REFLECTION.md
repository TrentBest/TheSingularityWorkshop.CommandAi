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

- **Latest verified implementation baseline before this reflection refresh:** [run 41711472222](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41711472222) completed successfully for exact head `3563bfcabdae9e4166c6b9f86a65edf959ab5631` on 2026-11-03. The `build-and-test` check succeeded; `publish_nuget` was skipped. The documentation refresh commit itself requires its own exact-head CI result.
- The immediately preceding source-fix head `14a0828fbce994eddef9f9d229afbbb8e3b16e8a` also passed [run 41594167943](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41594167943).
- These runs verify the null-command validation-order fix and its regression test. A null command in the proposal input now produces the intended `ArgumentException` (with parameter name `commands`) rather than an accidental `NullReferenceException` during aggregate literal-size inspection.
- The earlier full workflow passed on exact development head `339f404b0c7ad0170ca1549da1161938047c1ca8`: [run 41592630562](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41592630562). It completed Workshop Markdown branding validation, .NET restore/build, tests with coverage, Codecov upload, quickstart execution, package pack, packed-artifact audit, and consumer compilation against the packed package.
- The `publish_nuget` job was skipped. The package artifact was produced but not published.
- The verified implementation includes adversarial codec tests and deterministic-LF encoding. The encoder emits LF line separators on every platform; the decoder accepts LF and CRLF.
- The approved package version remains `0.1.0-alpha.2`. No package/protocol version increment or NuGet publication occurred.

## Latest development activity (2026-11-03)

- Development implementation baseline `3563bfcabdae9e4166c6b9f86a65edf959ab5631` (`ci: retain verified Codecov token upload`) passed exact-head workflow run [41711472222](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41711472222). The latest documentation refresh, exact head `19b882c52864cb1865c5fded0630eac6e2413441`, also passed [run 41721918057](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41721918057): build, tests/coverage, Codecov, quickstart, package pack, and packed-package consumer audit succeeded; `publish_nuget` was skipped.
- The CiAi observer workflow is now present on **both** `master` and `development`. The files have the same blob SHA (`95328575a903121248b49f83b9170673d0b88164`), and the master copy arrived through merged PR #5 at merge commit `181b0ef6a9c3ffb779a94898cf2c0653ff3ac8e9`.
- PR #5 is closed and merged. The first automatic observer run [41721956351](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41721956351) completed successfully. Its `commandai-ciai-evidence` artifact (ID `13272482468`) contains JSON and Markdown reports. The report observed candidate `4141b2776a79133c998ce2b4e94d6f69c199dcd4`; discovery was complete, the required `.github/workflows/build.yml` evidence was complete and terminal, and the matching `CommandAi CI` run [41721841220](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41721841220) passed with complete job/step evidence. The `publish_nuget` job was skipped. This is read-only CI observation, not release-readiness certification or publication authorization.
- The source branch `ci/register-ciai-observer` remains after the merge and is safe for the owner to delete manually. No branch was deleted.
- Current branch comparison: `development` is 262 commits ahead and 4 behind `master`; GitHub reports the refs as diverged. The compare includes the development implementation/docs and the master-only workflow merge history. Do not reset, force-push, merge, or otherwise reconcile these refs without a dedicated ancestry/content audit and explicit authorization.
- Current branch inventory: `master`, `development`, `ci/register-ciai-observer`, and `fix/commandai-v1-contract-docs`. There are no open CommandAi pull requests. The fix branch should be compared with current development before the owner removes it.
- Approved CommandAi/1 source and API remain the only supported contract. No /2-only fields or compatibility parser are to be added.
- The earlier exact-head CI runs continue to establish the current model/codec test baseline: [41594167943](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41594167943) and [41594181593](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41594181593). The full package/consumer validation run remains [41592630562](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41592630562).
- Package version remains `0.1.0-alpha.2`; no package/protocol version increment or NuGet publication occurred.

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

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

- Last confirmed full workflow: [run 41391559191](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41391559191) — success on commit `a6615956d59633e290d6e86139bb2940abad918b`. It completed branding validation, restore, build, tests with coverage, Codecov upload, quickstart, pack, packed-artifact audit, and consumer compilation. The NuGet publication job was skipped.
- That run does **not** verify the later adversarial test additions or subsequent metadata/status corrections. Exact-head CI for the current development state is not yet confirmed.
- Earlier source/test verification at `e1e7d183b1526c4f4fd0ec9cdac851243f4b97a8` passed run 41391145708; it predates the later adversarial tests.
- Do not describe the current head as CI-green until a workflow run for that exact commit completes successfully.

## Latest development activity (2026-11-03)

- Approved /1 source and API restoration remains the basis of the package.
- Adversarial codec and boundary tests were added in `b1f4abae7dc708f0345747c280eec22edc18e5f2`: malformed JSON literals, nested/trailing envelope content, oversized decode/encode envelopes, and configured command/argument boundaries.
- The domain matrix was corrected in `084dbe7ebcc9a1f224a36c22c86a835e19a3a5d3` to distinguish /1 exclusions from implemented fields and to keep correlation/provenance and actual outcomes with ProtocolAi/runtime.
- Documentation status-head references were corrected in `b30c6cd933a7ddf5498c62b74d6c1fcbc7d3a962`.
- Package metadata was corrected in `a92610be1b2c3d0b5b5f5d4cc5e6e0799e1ace14`; its description now accurately identifies the CommandAi/1 envelope rather than the superseded /2 candidate.
- Package version remains `0.1.0-alpha.2`; no package/protocol version increment or NuGet publication.

## Remaining work

1. Obtain and record exact-head CI for the current /1 source, adversarial tests, documentation, and package metadata.
2. Finish the source-to-doc audit across all public type guides and UML pages; remove any stale /2-only references and verify the required moniker/footer on every Markdown page.
3. Complete the optionality/default/schema and compatibility-policy review without inventing unsupported /1 semantics.
4. Coordinate cross-repository ownership for observer registration, event correlation/replay, and evidence-bound gate transitions through the relevant issues.
5. Keep CiAi PR #37 draft until its submodule pin and description reference the approved /1 source and its required workflows pass on the exact updated PR head.

No user-held decision currently blocks the /1 source audit. Cross-repository runtime semantics remain agent-owned coordination work; they do not block the package's current /1 representation review.

No NuGet publish, merge, release, deployment, master alteration, or branch deletion is authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

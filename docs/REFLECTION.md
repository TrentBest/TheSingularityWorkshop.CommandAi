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

- <badge>Latest full workflow passed</badge> [Run 41591997190](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41591997190) completed successfully on exact development head `8c2bf30a479a107271c914f0eb3e5e4228188d93`.
- The run completed Workshop Markdown branding validation, .NET restore/build, tests with coverage, Codecov upload, quickstart execution, package pack, packed-artifact audit, and consumer compilation against the packed package.
- The `publish_nuget` job was skipped. The package artifact was produced but not published.
- This verification includes the adversarial tests added in `b1f4abae7dc708f0345747c280eec22edc18e5f2` and the corrected CommandAi/1 package metadata.
- Earlier source/test verification at `e1e7d183b1526c4f4fd0ec9cdac851243f4b97a8` passed run 41391145708; the later full workflow supersedes it.

## Latest development activity (2026-11-03)

- Approved /1 source and API restoration remains the basis of the package.
- Adversarial codec and boundary tests were added in `b1f4abae7dc708f0345747c280eec22edc18e5f2`: malformed JSON literals, nested/trailing envelope content, oversized decode/encode envelopes, and configured command/argument boundaries.
- The domain matrix was corrected in `084dbe7ebcc9a1f224a36c22c86a835e19a3a5d3` to distinguish /1 exclusions from implemented fields and to keep correlation/provenance and actual outcomes with ProtocolAi/runtime.
- Documentation status-head references were corrected in `b30c6cd933a7ddf5498c62b74d6c1fcbc7d3a962`.
- Package metadata was corrected in `a92610be1b2c3d0b5b5f5d4cc5e6e0799e1ace14`; its description now accurately identifies the CommandAi/1 envelope rather than the superseded /2 candidate.
- The status page is being updated after the successful exact-head run; its own documentation-only commit is expected to trigger the same push workflow.
- Package version remains `0.1.0-alpha.2`; no package/protocol version increment or NuGet publication.

## Remaining work

1. Confirm the workflow triggered by this status-page-only update.
2. Finish the source-to-doc audit across all public type guides and UML pages; remove any stale /2-only references and verify the required moniker/footer on every Markdown page.
3. Complete the optionality/default/schema and compatibility-policy review without inventing unsupported /1 semantics.
4. Coordinate cross-repository ownership for observer registration, event correlation/replay, and evidence-bound gate transitions through the relevant issues.
5. Keep CiAi PR #37 draft until its submodule pin and description reference the approved /1 source and its required workflows pass on the exact updated PR head.

No user-held decision currently blocks the /1 source audit. Cross-repository runtime semantics remain agent-owned coordination work; they do not block the package's current /1 representation review.

No NuGet publish, merge, release, deployment, master alteration, or branch deletion is authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

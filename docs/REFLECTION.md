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

- **Latest full workflow passed:** [run 41592630562](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41592630562) completed successfully on exact development head `339f404b0c7ad0170ca1549da1161938047c1ca8`.
- The run completed Workshop Markdown branding validation, .NET restore/build, tests with coverage, Codecov upload, quickstart execution, package pack, packed-artifact audit, and consumer compilation against the packed package.
- The `publish_nuget` job was skipped. The package artifact was produced but not published.
- This verification includes the adversarial codec tests and the deterministic-LF encoding regression test. The encoder emits LF line separators on every platform; the decoder accepts LF and CRLF.
- The approved package version remains `0.1.0-alpha.2`. No package/protocol version increment or NuGet publication occurred.

## Latest development activity (2026-11-03)

- Approved CommandAi/1 source and API remain the only supported contract.
- Adversarial codec and boundary tests were added in `b1f4abae7dc708f0345747c280eec22edc18e5f2`.
- The domain matrix was corrected in `084dbe7ebcc9a1f224a36c22c86a835e19a3a5d3` to distinguish /1 exclusions from implemented fields and to keep correlation/provenance and actual outcomes with ProtocolAi/runtime.
- Package metadata was corrected in `a92610be1b2c3d0b5b5f5d4cc5e6e0799e1ace14`; its description now accurately identifies the CommandAi/1 envelope.
- Deterministic LF text encoding was implemented in `1b7211d2638de680a75edddced78f6a7a60ef4e0`, with an exact-envelope regression test in `c59601eb940952c51e09344224d3c95d0f422c61`.
- The complete Markdown inventory was audited: all 31 Markdown pages have the animated moniker and canonical Workshop PNG footer. Removed /2-only model type references are absent; remaining /2 mentions are historical/correction notes.
- CiAi PR #37 merged at `49a802fcab60fa7411b01892072d7e2c8a2c72a9`. Draft CiAi PR #50 advances its submodule pin to `339f404b0c7ad0170ca1549da1161938047c1ca8`; both required workflows passed on exact PR head `6499e3a301dfeeeeafaff793ee392120efadedae` (runs 41592774387 and 41592774359). PR #50 remains open/draft; no merge or publication was performed.
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

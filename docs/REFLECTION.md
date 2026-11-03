<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and release-readiness evidence

## Approved direction

**Retain CommandAi/1.** The Workshop owns the current consumers, so internal repositories adapt together to the existing /1 model and wire shape. Do not migrate to /2, add /2-only fields, or bump the package/protocol version merely for internal coordination.

Package version remains `0.1.0-alpha.2`. NuGet publication is not authorized.

## Current implementation

- Immutable `CommandProposal` and `CommandInstruction` model.
- Positive integer action and parameter identities.
- Ordered typed arguments using integer identifiers or literal text.
- Generic fluent proposal and instruction builders.
- Line-oriented CommandAi/1 codec.
- Bounds for envelope, command count, arguments, and literal text.
- Decoder rejects unsupported versions, malformed envelopes, duplicate protocol declarations, duplicate parameters, and invalid values.
- Host owns domain meaning, authorization, approval, execution, and actual outcomes.

## Verification status

- Source revision: see the current `development` branch head.
- Most recent full CI run: [run 41391145708](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41391145708) — pending at the time this status page was last reviewed.
- Previous run 41390624598 built successfully and passed the branding check, but one malformed-envelope test case was incorrectly authored as a valid command. The test fixture was corrected in the next commit; the new run must complete before claiming green.
- The publish job is configured to remain skipped unless the explicit publication gate is satisfied.

## Latest development activity (2026-11-03)

- Latest source/test and domain-matrix audit head: `084dbe7ebcc9a1f224a36c22c86a835e19a3a5d3`.
- Latest documentation-only head (this status update): `e1af3e7362fc026db0f52b966888faf9087a4fe9`.
- Added adversarial codec and boundary tests in `b1f4abae7dc708f0345747c280eec22edc18e5f2`.
- Clarified the domain-completeness matrix so the current owner-approved /1 exclusions are not confused with implemented structured fields.
- Recorded the source audit and remaining cross-repository ownership questions in CommandAi issue #3.
- The previously successful full CI run (41391559191) verifies the earlier head `a6615956d59633e290d6e86139bb2940abad918b`; it does **not** verify the two newer commits. Exact-head CI for `084dbe7` remains unconfirmed.
- No package version change or NuGet publication.

## Remaining work

1. Complete CI on the corrected /1 source.
2. Finish the source-to-doc audit, including public type guides and UML pages; remove obsolete /2-only type references.
3. Update CiAi PR #37 to pin the exact verified /1 source and rerun its required workflows. Keep the PR draft.
4. Complete issue #3's domain completeness review with evidence and owner dispositions.
5. Review package contents and release readiness after all checks pass.

No user-held decision currently blocks the /1 restoration. No NuGet publish, merge, release, deployment, master alteration, or branch deletion is authorized.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

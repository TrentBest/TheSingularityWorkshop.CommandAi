<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and open questions

## Current implementation on development

- Immutable proposal and complete atomic instruction contracts.
- Required command identity, action identity, target, scope, idempotency key, and expected outcome contract.
- Ordered typed parameters and optional grammar-versioned opaque preconditions.
- Bounded outcome fields, preconditions, literal values, aggregate text, command count, and envelope size.
- CommandAi/2 JSON command entries with strict version, property, and value-shape validation.
- Generic fluent proposal/instruction builder.
- Focused tests for round-trip fields, malformed envelopes, duplicate identities, bounds, and builder snapshots.
- Console quickstart with host-owned named command extensions.
- No runtime package references to sibling AI packages.
- No command execution, authorization, durable observer lifecycle, evidence evaluation, or outcome lifecycle.

## Ownership boundaries

CommandAi describes the proposed atomic command. GrammarAi owns the structure of precondition expressions. ProtocolAi owns protocol-qualified vocabulary and correlated/provenance-bearing actual outcome events. CiAi owns CI evidence and verdicts. FSM_Foundry and the host own live-state evaluation, idempotency persistence/enforcement, durable observers, authorization, approval, scheduling, dispatch, execution, and runtime outcome lifecycle.

A declared expected outcome is not an actual outcome. A parsed command is not authorized. An idempotency key is not replay protection until the host enforces it.

## Compatibility decision

CommandAi/1 is intentionally removed from the supported contract. The package has not been externally released, so the model and wire format are being corrected as a breaking change rather than carrying a compatibility parser or adapter. Consumers in the Workshop repositories must migrate together before any package publication.

## Current evidence and active domain review

- [Domain completeness and ownership inventory](DOMAIN_COMPLETENESS.md) now distinguishes implemented representation guarantees, opaque fields owned by other packages, intentionally excluded runtime/presentation concerns, and unresolved contract gaps.
- All 34 Markdown pages present at the initial audit had the animated moniker at the top and the canonical Workshop PNG footer. CI now enforces both requirements for every Markdown file in the repository.
- Coverage collection and optional Codecov upload are part of the CommandAi workflow. The workflow passed on development commit `7835481ae444f0472a9ff974557558d23274ab5d`, including branding validation, build, adversarial tests with coverage, successful Codecov upload, quickstart, package pack, packed-artifact audit, packed-package consumer compilation, and package artifact upload. The publication job was skipped.
  - [Exact verified workflow run](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41361427117)
- [Command-domain completeness review](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/issues/3) tracks proposal/correlation identity, parameter schemas, workflow semantics, strict codec compatibility, and education/presentation neutrality.
- [GUI.SVG documentation-generation coordination](https://github.com/TrentBest/TheSingularityWorkshop.GUI/issues/69) asks for a supported docs-only path to generate source-derived SVGs without adding a GUI runtime dependency to CommandAi.

## Remaining verification and decisions

1. Complete the domain review and agree with ProtocolAi/Foundry on ownership before changing CommandAi/2 wire fields.
2. Determine whether proposal identity, causal/correlation identity, or parameter schema metadata have enough generic consumer value to justify a versioned contract change.
3. Verify deterministic serialization across operating systems before describing the current text codec as canonical.
4. Add adversarial tests for remaining required-field/type combinations and resource-boundary edges.
5. CiAi PR #37 now composes CommandAi/2 and both of its required workflows pass on PR head `2ae55ce7775d8f15ad4e5dcb83484fe1a76d6976`. However, its actual submodule gitlink is `d35845113dd67a0b8a3c4654c0e095146188f916`, 18 commits behind this verified CommandAi head. A follow-up request is posted on the PR to advance the pin, correct its stale summary, and rerun both workflows against the exact current CommandAi source. Keep the PR draft.
6. GUI issue #69 requests a docs-only GUI.SVG generation path. No response has been posted yet; do not claim GUI.SVG-generated diagrams are integrated.
7. Re-run the complete workflow on the final release candidate and review package metadata and contents.

No NuGet publication, master merge, or release is authorized by this status page.


---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

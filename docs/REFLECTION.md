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

## Remaining verification

1. Run the complete CommandAi workflow on the latest development head, including package artifact audit and quickstart compilation against the packed package.
2. Migrate The and the CiAi CommandAi proposal-composer feature branch to the new public contract.
3. Verify cross-repository project/submodule references and tests after the source migration.
4. Update any remaining docs, examples, fixtures, and protocol prompts that describe the previous ActionId-plus-Arguments model.
5. Keep NuGet publication disabled until the owner explicitly authorizes a package/version.

No NuGet publication is authorized by this status page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

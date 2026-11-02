<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Internal consumer alignment: retain CommandAi/1

## Owner decision

The repository owner has explicitly chosen to retain the existing CommandAi/1 model and wire format. The Workshop owns the current consumers, so internal repositories must adapt together. Do not migrate consumers to /2, do not add /2-only fields, and do not increment the package or protocol version merely for internal coordination.

This page replaces the former /2 migration proposal. It is retained at its existing path to avoid breaking links; it is not an instruction to migrate to /2.

## Approved model

The current API uses:
- `CommandProposalBuilder` to create an immutable proposal;
- `AddCommand(actionId, configure)` to add an instruction;
- `CommandInstructionBuilder` to add typed parameters;
- `WithIdentifier(parameterId, valueId)` for positive integer identifiers;
- `WithText(parameterId, value)` for literal text;
- `CommandAiTextCodec` for the line-oriented CommandAi/1 envelope.

## Consumer action

1. Remove assumptions about command IDs, targets, scopes, idempotency keys, precondition fields, and expected-outcome contracts; those are not fields in the approved /1 model.
2. Build named operations in the consuming host and translate them into action and parameter identities.
3. Keep host-owned policy, authorization, execution, and outcome handling outside CommandAi.
4. Update fixtures and tests to the /1 line-oriented envelope.
5. Run the consuming repository's restore, build, tests, and integration workflows against the exact CommandAi source revision.

## Compatibility and release

No external compatibility boundary is being introduced by this internal correction. Preserve package version `0.1.0-alpha.2`. No NuGet publication, master merge, release, or deployment is authorized by this page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

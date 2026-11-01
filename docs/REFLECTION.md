<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and open questions

## Current implementation

- Immutable proposal, instruction, argument, and typed-value contracts.
- Generic fluent `CommandProposalBuilder` and scoped `CommandInstructionBuilder`; application-specific named commands remain host/domain extensions.
- Explicit command, argument, per-literal, aggregate-literal, and encoded-envelope resource bounds.
- Line-oriented CommandAi/1 encoder/decoder.
- Positive-integer validation and duplicate-parameter rejection.
- Round-trip, malformed-input, builder behavior, immutable-snapshot, and resource-boundary unit tests.
- Console quickstart.
- No runtime package references to the other AI packages.
- No command execution, approval UI, authorization, or AI endpoint.

## Known limits

- Identifier meaning is host-defined and not resolved here.
- The initial wire format is intentionally narrow and does not support nested objects or lists as values.
- The only implemented envelope is `CommandAi/1`; version negotiation and migration policy are not implemented.
- GitHub Actions run [#63](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41280904172) verified the Release build, all 20 tests, quickstart execution, and pack step at commit [`e998450`](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/e99845013edf285ce0d86ecf667db77277035432). Later changes in this sequence added type/UML documentation and were separately verified by [run #68](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41281201023). The package has not been validated against a live provider or end-to-end host integration.

## Next evidence to gather

1. Add differential compatibility fixtures from the [legacy audit](LEGACY_COMPATIBILITY.md); the old decoder uses fixed action tables and supports only Surface/Repository, while the new package deliberately leaves identifier meaning external.
2. Cross-check the current ProtocolAi/GrammarAi contracts before introducing a package dependency or claiming end-to-end interoperability.
3. Decide whether a separate host/domain extension package should own shared named command definitions; do not add universal meanings to CommandAi.

No NuGet publication is authorized by this status page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

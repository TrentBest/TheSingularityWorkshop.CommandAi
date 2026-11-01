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
- GitHub Actions run [#89](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41283845342) verified the exact development head at commit [`96adbaa`](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/96adbaa023fd5a052479d809a2061801790e9f6a): Release build, all 20 tests, quickstart execution, package packing, packed-artifact audit, and compilation of the quickstart against the packed package. The NuGet publication job was skipped. The quickstart now compiles the host-owned named-command extension example. The package has not been validated against a live provider or end-to-end host integration.

## Next evidence to gather

1. Add a small host-owned integration example that composes the current ProtocolAi vocabulary, GrammarAi structure, and CommandAi proposal model without coupling their generic packages unnecessarily.
2. Exercise a domain-specific CommandProposalBuilder extension in tests, proving that named commands and semantic validation remain outside CommandAi while the result round-trips through the strict codec.
3. Keep authorization, approval, and execution in the host; successful proposal construction or encoding is not permission to act.

Legacy compatibility is intentionally not planned. The source-level observations are retained in [Legacy CommandAi — historical reference](LEGACY_COMPATIBILITY.md); no compatibility parser or fixture corpus should be built.

No NuGet publication is authorized by this status page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

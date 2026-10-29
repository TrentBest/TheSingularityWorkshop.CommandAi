<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi status and open questions

## Current implementation

- Immutable proposal, instruction, argument, and typed-value contracts.
- Line-oriented CommandAi/1 encoder/decoder.
- Positive-integer validation and duplicate-parameter rejection.
- Round-trip and malformed-input unit tests.
- Console quickstart.
- No runtime package references to the other AI packages.
- No command execution, approval UI, authorization, or AI endpoint.

## Known limits

- Identifier meaning is host-defined and not resolved here.
- The initial wire format is intentionally narrow and does not support nested objects or lists as values.
- Protocol version negotiation and compatibility policy are not implemented.
- The unit tests are authored but must be confirmed by CI before claiming verified behavior.
- Per-type UML/use-case coverage and automated documentation inventory are not yet present.

## Next evidence to gather

1. Restore, build, test, and run the quickstart on the repository's supported SDK.
2. Add compatibility fixtures based on the legacy CommandAi implementation in The, after reviewing the action/parameter vocabulary and intended migration.
3. Cross-check the current ProtocolAi/GrammarAi contracts before introducing a package dependency or claiming end-to-end interoperability.
4. Expand tests for quoted values, escaped quotes/backslashes, malformed tokens, and envelope boundary cases.
5. Add source-backed per-type documentation and UML/use-case companions as the domain stabilizes.

No NuGet publication is authorized by this status page.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

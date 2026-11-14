<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandGrammarBody

An immutable opaque byte payload paired with a positive GrammarAi wire-version identifier. It lets an enriched atomic command carry grammar-owned structure without making CommandAi depend on or interpret GrammarAi.

## Contract

- `GrammarWireVersion` must be positive.
- The payload must be non-empty and no larger than 32 KiB per command.
- The proposal aggregate is limited to 128 KiB of decoded GrammarAi body bytes.
- The byte payload is defensively copied and exposed as read-only memory.
- The text codec uses Base64 and rejects non-canonical Base64 representations.
- CommandAi validates only framing, canonical encoding, and resource bounds. GrammarAi owns decoding and structural validation.
- A valid body is still untrusted input; it does not establish truth, authority, or permission to execute.

Source: [CommandGrammarBody.cs](../../CommandGrammarBody.cs). Related: [CommandPrecondition](CommandPrecondition.md), [CommandInstruction](CommandInstruction.md), GrammarAi.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

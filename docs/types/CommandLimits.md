<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandLimits

## Purpose

Limits applied to command proposals, instructions, literal text, and encoded envelopes.

## Declaration and ownership

- **Declaration:** public static class `CommandLimits`.
- **Source:** [CommandLimits.cs](../../CommandLimits.cs).
- **Responsibility:** Centralizes explicit, testable representation limits so callers can reason about bounded input.
- **Non-goals:** These constants do not establish semantic validity, authorization, or execution safety.

## Contract and use

The current limits are: 256 commands per proposal; 64 arguments per command; 16,384 characters per literal; 1,048,576 aggregate literal characters per proposal; and 262,144 characters per encoded envelope. The proposal model and codec enforce the applicable bounds.

The public constants are enforced by the owning constructors/builders. Treat them as part of the current representation contract; do not silently bypass them in host adapters.

## Verification

See the package test project for boundary and over-limit regression cases.

## Related documentation

- [Companion UML and use cases](./CommandLimitsUML.md)

- [Problem domain](../PROBLEM_DOMAIN.md)
- [Documentation index](../../DOCUMENTATION_INDEX.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandLimits — UML and use cases

## Class neighborhood

~~~mermaid
classDiagram
 class CommandLimits {
  <<static>>
  +MaxEnvelopeCharacters = 262144
  +MaxCommands = 256
  +MaxArgumentsPerCommand = 64
  +MaxPreconditionsPerCommand = 32
  +MaxOutcomeFields = 64
  +MaxLiteralCharacters = 16384
  +MaxGrammarVersionCharacters = 128
  +MaxPreconditionCharacters = 16384
  +MaxIdempotencyKeyCharacters = 256
  +MaxTotalLiteralCharacters = 1048576
 }
~~~

## Use case

1. Construct a compact or enriched command through the public API.
2. The owning type checks collection and text limits before retaining input.
3. The codec checks the serialized envelope and enriched JSON nesting depth.
4. The caller receives an argument or format error when a bound is exceeded.

## Boundary

The limits bound representation size. They do not validate application meaning, evaluate preconditions, enforce idempotency, grant permissions, or authorize downstream execution. The aggregate in-memory literal bound is independent of the smaller encoded-envelope bound.

Source: [CommandLimits.cs](../../CommandLimits.cs).
---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

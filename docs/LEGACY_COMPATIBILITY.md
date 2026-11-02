<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Legacy CommandAi — historical reference

## Status

This page records observations about an earlier implementation. It is not a compatibility target, and the modern package does not claim drop-in interoperability with it.

## Source observations

The legacy implementation reviewed at [The / Architect/Workshop/Ai/CommandAi.cs](https://github.com/TrentBest/The/blob/6ad1971f3932654ea58f8dd8dbc0dde530e4e4c6/Architect/Workshop/Ai/CommandAi.cs) mixed generic parsing with application-specific vocabulary and approval/execution concerns. These observations are historical source notes, not a normative wire specification.

## Current package boundary

The current package uses the CommandAi/1 model: immutable ordered proposals, action identifiers, typed parameter values, bounded literal text, generic builders, and a line-oriented codec. It intentionally does not resolve identifier meanings, authorize, approve, or execute commands.

The repository's earlier /2 candidate is not the approved contract. Internal consumers must retain /1; do not add /2-only fields or version bumps merely for internal coordination.

For current implementation guidance, see the [theory chapter](THEORY.md), [builder integration](BUILDER_INTEGRATION.md), and [usage guide](USAGE.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

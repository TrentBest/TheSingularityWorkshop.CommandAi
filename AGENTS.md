<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Repository instructions for AI agents

## Before changing code

1. Read the [shared Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md).
2. Read CommandAi's problem domain, architecture, theory, and status/open questions.
3. Check the current branch head and CI before making assumptions.
4. Read source and tests before changing public contracts.
5. Keep source-implemented behavior, planned work, and verified CI evidence clearly separated.

## Branch and release policy

The permanent branch model is exactly `development` and `master`. Work directly on `development`; the owner handles deletion of branches when explicitly identified as ready. Never delete a branch on the owner's behalf.

Do not modify `master`, merge pull requests, publish to NuGet, deploy, or release without explicit authorization for that specific operation. Keep publication disabled by default in CI.

## Domain boundaries

- ProtocolAi owns shared vocabulary identity and broader causal/event/outcome provenance.
- GrammarAi owns grammar definitions and grammar-specific structural validation.
- CommandAi owns generic proposal/exchange representations, descriptors, typed values, structural bounds, and local request/response correlation.
- HeadlessAi owns configured endpoint communication.
- CiAi and consuming applications own domain-specific definitions and mappings.
- AppAi may later own application-program composition and response planning.
- FSM_Foundry / host owns live-state resolution, policy, authorization, approval, lifecycle, dispatch, and execution.

CommandAi must not depend on GrammarAi or execute commands. Address IDs are contract identifiers, never pointers. Grammar metadata is opaque to CommandAi. A structurally valid response is still untrusted intent.

## Version policy

The `CommandAi/1` protocol identity may evolve in place by owner direction. It is distinct from the NuGet package version. Do not invent a /2 migration or increment a version merely because an internal contract expands. Coordinate consuming repositories and package publication through the owner's release workflow.

## Documentation claims

Never call an example tested unless a recorded run supports that claim. Do not claim wire support for an object model until a codec and tests exist. Do not add unverified badges or claim release readiness based only on a successful build.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

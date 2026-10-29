<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Repository instructions for AI agents

## Before changing code

1. Read the [shared Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md).
2. Read [CommandAi's problem domain](docs/PROBLEM_DOMAIN.md), [architecture](docs/ARCHITECTURE.md), [theory](docs/THEORY.md), and [status/open questions](docs/REFLECTION.md).
3. Check the current branch head and CI before making assumptions about the state of the work.
4. Read source and tests before changing public contracts.

## Branch and release policy

The permanent branch model is exactly:
- development — active work and the next release candidate.
- master — the previously released baseline until the owner-authorized release operation replaces it.

Work directly on development. Do not create persistent feature or handoff branches. Do not publish to NuGet unless the owner explicitly authorizes that specific release. Keep publication disabled by default in CI.

## Domain boundaries

- ProtocolAi owns vocabulary and symbol identity.
- GrammarAi owns grammar structure and composition.
- CommandAi owns versioned command-proposal representation and its codec.
- HeadlessAi owns configured endpoint request/response communication.
- The host owns trust decisions, policy, authorization, approval, and execution.

Do not make a lower-level package depend on an adjacent package merely because the concepts compose. Verify the actual public contracts before adding dependencies or claiming interoperability. CommandAi must not execute commands or grant authority.

## Documentation claims

Separate source-implemented behavior, source-checked examples, verified CI behavior, conceptual diagrams, and future work. Never call a local example tested unless a recorded run supports that claim. Do not add unverified badges or claim a package is release-ready based only on a successful build.

## Scope

CiAi is outside this repository's scope. Do not edit or coordinate changes in CiAi as part of CommandAi work.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

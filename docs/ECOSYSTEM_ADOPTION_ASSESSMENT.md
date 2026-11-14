<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi ecosystem adoption assessment

**Assessment date:** 2026-11-14  
**Repository inventory:** 100 repositories visible to the connected TrentBest GitHub account, including CommandAi; 99 potential consumers reviewed.  
**Purpose:** identify where CommandAi adds a real bounded atomic-command contract, rather than introducing a dependency merely because a repository contains AI, state machines, events, or application workflows.

## Executive assessment

The strongest adoption finding is **SingularityBot already consumes CommandAi**. Its development branch contains `docs/commandai-integration-v1.md`, a bot-owned vocabulary (`SingularityBot.Commands/1`), and a validator that accepts real CommandAi `CommandProposal` objects. This is a verified integration, not a future proposal. The remaining work is to connect validated proposals to existing goal/job contracts and prove the first end-to-end production-target use case without creating a second scheduler or allowing model output to dispatch SC2 actions.

The second candidate is **The (The Architect)**. Its README describes a terminal for understanding, composing, and eventually operating the software ecosystem through context. A bounded CommandAi proposal surface could be useful when the Architect turns an approved operator request into a sequence of typed, atomic domain actions. This is conditional: no inspected evidence yet establishes a current command-proposal implementation, action registry, or execution adapter. The first step should be an architecture issue and a narrow consumer proof, not an immediate package dependency.

**FSM_Foundry, ServerApp, and AnyApp are not recommended as direct CommandAi dependencies today.** Foundry and ServerApp currently expose only minimal placeholder READMEs in their inspected default-branch trees, with no source architecture to validate. Foundry may eventually host an application-specific CommandAi adapter, but the generic Foundry core should not own application command meaning or become an authorization shortcut. AnyApp currently documents a different semantic-intent boundary (ProtocolAi + GrammarAi + FSM_UserIO) and host-owned Experience execution; CommandAi should be introduced there only if a concrete bounded atomic-action proposal workflow is identified.

The overall rule: **CommandAi is a representation of proposed atomic intent, not a workflow engine, capability grant, authorization decision, scheduler, event bus, or execution engine.** Hosts own action meaning, live-state validation, policy, authorization, approval, deduplication enforcement, lifecycle, dispatch, and outcomes.

## Findings and recommendations

### 1. SingularityBot — adopted; complete the runtime bridge

**Disposition:** Adopted / integration in progress.  
**Evidence:** [SingularityBot CommandAi integration v1](https://github.com/TrentBest/TheSingularityWorkshop.SingularityBot/blob/development/docs/commandai-integration-v1.md), [architecture](https://github.com/TrentBest/TheSingularityWorkshop.SingularityBot/blob/development/docs/architecture.md).

The integration already:
- references the CommandAi project from the bot's development checkout;
- defines bot-owned stable action and parameter identities under `SingularityBot.Commands/1`;
- validates actual `CommandProposal` instances against bot-owned vocabulary;
- rejects unknown actions/parameters, missing required parameters, wrong value kinds, invalid counts, and out-of-range priorities;
- keeps SC2 protocol action construction/submission behind the SC2 adapter.

**Recommended next implementation sequence**
1. Resolve identifier-valued parameters against the active BotDefinition and match catalogs; structural identifier validity is not proof that a target exists.
2. Translate a validated `SetProductionTarget(Marine, N)` into the existing production goal/job contract.
3. Reuse the existing goal/job scheduler; do not introduce a second scheduler or a CommandAi-owned executor.
4. Add proposal identity/idempotency and lifecycle correlation in the bot/host layer so retries reconcile to existing objectives.
5. Record observed progress/completion against the originating proposal; a submitted SC2 action is not proof of completion.
6. Add an end-to-end consumer test proving invalid, stale, unknown, duplicate, and unauthorized proposals cannot dispatch actions, and that the live frame loop never waits for an LLM.

CommandAi remains a dependency-free contract library. The bot owns the action catalog and meaning; the runtime and adapter own semantic validation and execution.

### 2. The (The Architect) — conditional candidate

**Disposition:** Candidate; issue requested for a concrete first proof.  
**Evidence:** [The README](https://github.com/TrentBest/The/blob/master/README.md) describes an ecosystem command center intended to understand, compose, and eventually operate the Workshop.

**Potential use**
- Convert an operator-approved request into a bounded proposal of atomic actions such as inspect repository, prepare a documented change, request a build, or open a review.
- Keep each action typed and explicitly identified; use ordered arguments and stable action IDs owned by The or the domain adapter.
- Present the proposal for review before any privileged action.
- Pass approved proposals to a host adapter that resolves action IDs, validates current repository/ref/SHA and policy, then invokes the appropriate GitHub or local workflow service.
- Correlate results and lifecycle evidence outside CommandAi; do not treat a well-formed proposal or expected outcome as proof that work succeeded.

**Required proof before adoption**
- Identify one real Architect interaction that currently produces an action request.
- Define a small action vocabulary owned by The, not by CommandAi.
- Demonstrate proposal construction, strict validation, review/approval, host dispatch, and correlated result reporting.
- Prove default-deny behavior for unknown actions/arguments and stale repository/ref/SHA.
- Keep long-running workflows, retries, dependencies, and progress state in The/host composition, not inside one atomic proposal.

Do not add CommandAi as a dependency merely to encode existing navigation, semantic intents, or ordinary UI events.

### 3. FSM_Foundry — future adapter boundary, not a core dependency

**Disposition:** Defer direct adoption.  
**Evidence:** inspected default-branch tree contains only repository scaffolding, a minimal README, and Sith-policy assets; no executable source or architecture contract was available to review.

Foundry is an execution/runtime concern. If a concrete host later chooses to accept CommandAi proposals, the application-specific adapter should:
- resolve action IDs against a trusted application registry;
- validate current state and capabilities;
- enforce authorization and approval;
- translate approved proposals into existing Foundry/runtime work;
- own deduplication, lifecycle, dispatch, and actual outcomes.

Foundry should not infer application meaning from generic IDs, authorize a proposal because it parses, or make CommandAi a mandatory dependency of the generic runtime. Reassess after Foundry has a documented host/adapter contract and a real consumer.

### 4. ServerApp — future host candidate, not yet evidenced

**Disposition:** Defer.  
**Evidence:** inspected default-branch tree contains only repository scaffolding, a minimal README, and Sith-policy assets; no executable server/session contracts were available to inspect.

A future ServerApp command endpoint could accept a bounded CommandAi proposal from an authenticated client or trusted agent, but only after ServerApp defines:
- caller identity and authorization outside CommandAi;
- a server-owned action registry and request/session binding;
- replay/deduplication and durable lifecycle semantics;
- strict validation of current session state and allowed target scope;
- a separate event/result contract for reporting what actually happened.

Do not expose arbitrary remote command execution. A CommandAi proposal is data, not a remote shell or permission token.

### 5. AnyApp — no direct adoption identified yet

**Disposition:** Defer / existing semantic-intent contract is currently the better fit.  
**Evidence:** [AnyApp README](https://github.com/TrentBest/AnyApp/blob/master/README.md) documents host-owned Experience execution and a ProtocolAi + GrammarAi + FSM_UserIO semantic-intent path.

CommandAi would add value only if AnyApp introduces a distinct workflow in which an AI or other planner proposes a bounded set of atomic host actions (for example, an explicitly reviewable Experience-management operation). It should not replace `SemanticIntent`, manifest composition, or the existing vocabulary/grammar boundary. AnyApp remains the authority for Experience meaning, policy, and execution.

## Explicit non-adoptions and boundary decisions

These are not negative judgments about the repositories; the inspected purpose does not establish a CommandAi-shaped problem.

| Repository / group | Disposition | Reason |
|---|---|---|
| ProtocolAi | No dependency | ProtocolAi owns cross-boundary vocabulary identity and event/provenance contracts; it is a peer boundary, not a CommandAi consumer. |
| GrammarAi | No dependency | GrammarAi owns structural grammar and validation. CommandAi may carry opaque GrammarAi bytes but must not depend on or interpret GrammarAi. |
| FSM_COS | No dependency | Composition manifests and MicroBundle assembly are not atomic application command proposals. |
| FSM_API and FSM layer/integration repositories | No dependency indicated | State-machine lifecycle primitives are not command semantics; avoid pulling an application proposal model into the FSM substrate. |
| HeadlessAi and its examples/benchmarks | No direct dependency | HeadlessAi transports requests to configured endpoints; the host decides whether response content is a CommandAi proposal and validates it. |
| CiAi and CogA–CogE | No direct dependency indicated | CiAi's request, policy, result-correlation, and evidence contracts have explicit ownership and are not interchangeable with an application atomic-command proposal. |
| FSM_UserIO | No dependency indicated | SemanticIntent represents user/application intent routing, not a generic atomic action proposal. A host may translate between them where a concrete workflow requires it. |
| Experiences, MicroBundleRepository, MicroBundleDomain, MicroBundleIngestor, Workshop | No use identified | Artifact identity, publication, ingestion, and composition are not themselves command proposal domains. |
| GUI, Renderer, Visualize, Physics, Gestures, Learning, NeuralNetworks, DataWarehouse, benchmarks | No use identified | These are capability, simulation, rendering, learning, memory, or measurement systems. They may be invoked by an application host, but should not depend on CommandAi solely for that reason. |
| Profiles, Economy, CreatorNetwork, MyVR, WebApp, Hermit, SingularityWarehouse | Defer | No concrete atomic-command boundary was established from the inspected README-level evidence. Reassess if one develops a trusted action surface. |
| StoryBuilder | Watchlist only | A future campaign/authoring workflow may propose atomic authoring actions, but the inspected README does not establish a CommandAi consumer. Keep narrative composition and provenance in StoryBuilder. |
| SingularityBot | Already adopted | See the detailed finding above; do not create a second vocabulary or parallel integration. |
| Legacy demos, templates, benchmarks, archived-era experiments, and unrelated BIM/Revit/Unity repositories | No use identified | No current CommandAi-shaped problem was evident from their available repository descriptions. |

## Repository inventory reviewed

The connected account returned 100 repositories (including CommandAi). The 99 other repositories were included in the first-pass inventory; README content was retrieved where available, and targeted architecture/source documentation was inspected for the strongest candidates. A missing or minimal README is not evidence that a repository could never benefit; it is recorded as insufficient evidence rather than an inferred use case.

| Repository | First-pass disposition |
|---|---|
| Tutorials | No use identified |
| TrentCoRetailManager | No use identified |
| ConstructorBots | No use identified |
| Rankings | No use identified |
| RankingLists | No use identified |
| DynamoNodes | No use identified |
| B4G-Bot | No use identified |
| B4G_Bot | No use identified |
| B4GBot | No use identified |
| MyCity | No use identified |
| AmazonBIM | No use identified |
| DSD_BIM | No use identified |
| TheSingularityWorkshop | No use identified |
| AnyApp | Defer; current semantic-intent boundary |
| FSM_API_Unity | No use identified |
| FSM_API.ConsoleApp | No use identified |
| FSM_API | No use identified |
| TrentBest.github.io | No use identified |
| raWWar | No use identified |
| DoctorFSM | No use identified |
| DiceFSM | No use identified |
| FSM_UnityIntegrationAdvanced | No use identified |
| FSM_UnityIntegration | No use identified |
| Diagrams | No use identified |
| The | Conditional candidate; issue proposed |
| FSM_ProjectTemplates | No use identified |
| GitHackforFun | No use identified |
| FSM_Helix | No use identified |
| SimpleDemos | No use identified |
| MineField | No use identified |
| FSM_UnityCompatability | No use identified |
| WebPage | No use identified |
| FSM_LayerTemplate | No use identified |
| FSM_PackageManager | No use identified |
| FSM_Layer_Serialization | No use identified |
| FSM_Layer_Generics | No use identified |
| FSM_Layer_DependencyInjection | No use identified |
| FSM_Bot | No use identified; legacy bot lineage |
| FSM_Bot_Editor | No use identified |
| FTI_BIM | No use identified |
| TheForge | No use identified |
| ZeroTouchFTI | No use identified |
| FaithTechInc | No use identified |
| SingularityWarehouse | Defer; no command boundary established |
| FSM_Dungeon | No use identified |
| RevitFamilyManagerBuilders | No use identified |
| Agents.LLM | No use identified from available README |
| AnyApp-2026-06-28_19-34-37 | No use identified; historical snapshot |
| ShowcasePong | No use identified |
| SingularityShipBuilder | No use identified |
| SingularitySoldiers | No use identified |
| SingularityBunnies | No use identified |
| SingularityWorkshopAcademy | No use identified |
| Workshop | No use identified; MicroBundle artifact |
| FSM_API_Benchmark | No use identified |
| TheSingularityWorkshop.GUI | No use identified |
| FSM_COS | No dependency |
| FSM_REST | No use identified |
| FSM_Serialization | No use identified |
| MicroBundleDomain | No use identified |
| FSM_Serialization_Benchmarks | No use identified |
| MicroBundleDomain_Benchmarks | No use identified |
| ProtocolAi | No dependency |
| GrammarAi | No dependency |
| MicroBundleRepository | No use identified |
| Experiences | No use identified |
| WebApp | No use identified |
| MyVR | Defer |
| FSM_UserIO | No dependency indicated |
| Ontology | No use identified |
| Renderer | No use identified |
| Renderer.Benchmarks | No use identified |
| Profiles | Defer |
| Economy | Defer |
| MicroBundleIngestor | No use identified |
| HeadlessAi | No direct dependency |
| HeadlessAi.AnyAppExample | No direct dependency |
| HeadlessAi.Benchmarks | No direct dependency |
| Visualize | No use identified |
| Gestures | No use identified |
| Physics | No use identified |
| StoryBuilder | Watchlist only |
| FSM_Foundry | Defer; no implementation evidence in inspected tree |
| CiAi | No direct dependency indicated |
| CiAi.CogA | No direct dependency |
| CiAi.CogB | No direct dependency |
| CiAi.CogC | No direct dependency |
| CiAi.CogD | No direct dependency |
| CiAi.CogE | No direct dependency |
| FSM_COS_Benchmarks | No use identified |
| DataWarehouse | No use identified |
| Benchmarks.DataWarehouse | No use identified |
| CreatorNetwork | Defer |
| CommandAi | Assessment owner; excluded from consumer count |
| Learning | No use identified |
| ServerApp | Defer; no implementation evidence in inspected tree |
| Benchmarks.GUI | No use identified |
| Hermit | Defer |
| FSM_NeuralNetworks | No use identified |
| SingularityBot | Adopted; integration exists |

## Integration checklist for any new consumer

1. **Use case first:** name one real application operation that benefits from a bounded atomic proposal.
2. **Consumer-owned vocabulary:** action IDs, parameter IDs, names, and meanings belong to the application/adapter, not CommandAi.
3. **Build proposals:** use CommandAi immutable proposal/builder APIs and bounded typed arguments; preserve deterministic ordering and strict validation.
4. **Validate in the host:** resolve IDs, required/optional semantics, target existence, current state, scope, and capability against trusted application data.
5. **Human/policy gate:** authorization and approval are external to CommandAi. Never infer permission from successful decoding.
6. **Execute through existing runtime:** translate approved proposals into existing domain jobs/state machines/adapters; do not create a second scheduler.
7. **Track lifecycle separately:** correlation, idempotency enforcement, retries, cancellation, actual outcomes, and evidence belong to the host/protocol/workflow layer.
8. **Prove fail-closed behavior:** unknown action/parameter, malformed values, stale state, unauthorized caller, duplicate delivery, partial failure, and unknown outcome must not be upgraded to success.
9. **Keep package boundaries lean:** no automatic CommandAi → ProtocolAi, GrammarAi, FSM_API, FSM_COS, or provider dependency.
10. **Release only after consumer evidence:** use the development branch for integration and validate against the exact candidate source/package. No publication or merge is implied by this report.

## Method and limitations

This is a first-pass ecosystem inventory, not a full code audit of every repository. The connected GitHub account returned 100 repositories; all 99 other names were inventoried and README content was retrieved where available. Targeted deeper review focused on SingularityBot, AnyApp, FSM_COS, HeadlessAi, CiAi, ProtocolAi, GrammarAi, FSM_Foundry, and ServerApp. Several older repositories have missing/minimal READMEs, and Foundry/ServerApp currently expose only scaffolding on the inspected default branch. Those results are marked as no-evidence/defer rather than definitive permanent rejection.

A future pass should deepen source inspection when a repository develops a real proposal producer, trusted action registry, or execution adapter. The decision standard remains concrete consumer value plus a fail-closed test—not architectural fashion.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

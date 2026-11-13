<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi and SingularityBot: from strategic intent to governed game actions

> **Conceptual integration guide**
>
> This document describes a proposed integration between CommandAi and a SingularityBot-based StarCraft II ecosystem. It is an architectural walkthrough, not a claim that the complete adapter, ladder host, or end-to-end execution path already exists.

## 1. The idea

**SingularityBot decides; CommandAi describes the proposed action; the trusted host validates and authorizes it; the StarCraft II adapter translates it into legal game commands; observations and outcomes return to the bot.**

CommandAi should not become a StarCraft bot. It should be a stable boundary between a decision-maker and the system allowed to cause effects.

There is an important distinction in this integration: **the most natural first role for CommandAi is to represent high-level orders or goal requests that are resolved into SingularityBot's existing goal/FSM model.** It should not automatically sit in the innermost per-frame unit-control loop. The bot already collects intents, arbitrates them, validates them against current observations and game data, and translates accepted intents into SC2 actions. That deterministic pipeline remains authoritative.

SingularityBot should remain free to evolve its strategies, state machines, unit behaviors, and learning systems without making the generic command contract understand Terran build orders, Protoss abilities, Zerg inject cycles, or StarCraft's action API. CommandAi should not need to know what a Marine is.

That separation makes the same platform useful for a single experimental bot, a tournament ladder, a multi-team match, or a community of independently developed bots.

## 2. The participants

| Participant | Responsibility | Must not become responsible for |
|---|---|---|
| ExampleBot / SC2 adapter | Connect to StarCraft II, join games, collect observations, submit game actions, handle API details | Owning all strategy and tournament policy |
| FSM_API | Provide the state-machine heartbeat and lifecycle foundation | Knowing StarCraft-specific strategy |
| SingularityBot / FSM_Bot | Own match strategy, goal resolution, economy, army behavior, unit-level decisions, and adaptation | Bypassing the validated action pipeline |
| CommandAi | Represent bounded, ordered, typed high-level orders or goal proposals | Owning StarCraft semantics, tactical FSM execution, or SC2 actions |
| Current SingularityBot action pipeline | Collect and arbitrate FSM intents, validate against current observations/game data, translate and submit SC2 actions | Accepting unvalidated or stale intent |
| FSM_Foundry (planned integration) | Become the shared composed-workload executor when integrated | Being described as already wired into the current bot |
| GrammarAi (where adopted) | Define and validate grammar-governed structures | Granting permission to act in the live game |
| Match supervisor / observer host | Provision and supervise independent player instances, enforce match rules, collect evidence and results | Sharing one player's authority with another |
| CiAi | Build, test, and evaluate integration evidence | Treating a successful build as proof of safe live execution |

Some components may initially run in one process. A tournament deployment may isolate them in separate processes or containers. The trust boundaries should remain the same.

## 3. The full loop

~~~text
                 MATCH SUPERVISOR
          provisions isolated player instances
                         |
             +-----------+-----------+
             |                       |
         Player A                 Player B
             |                       |
      ExampleBot / SC2         ExampleBot / SC2
             |                       |
       observations              observations
             |                       |
       FSM_API heartbeat         FSM_API heartbeat
             |                       |
       SingularityBot            SingularityBot
       strategic FSMs             strategic FSMs
             |                       |
       high-level order           high-level order
             |                       |
        CommandAi proposal         CommandAi proposal
             |                       |
       bot goal resolver          bot goal resolver
             |                       |
       strategic/FSM goals        strategic/FSM goals
             |                       |
       FSM intent pipeline        FSM intent pipeline
       arbitrate + validate       arbitrate + validate
             |                       |
       SC2 action adapter         SC2 action adapter
             |                       |
             +------ StarCraft II ---+
                         |
                  game simulation
                         |
                  next observations
                         |
               per-player feedback
                         |
                  next FSM tick
~~~

The supervisor coordinates the match, but each participant has its own runtime, command channel, and authority boundary. The CommandAi lane shown here is an optional high-level order ingress; the bot's ordinary internal FSM decisions can continue without it.

### 3.1 Startup and identity

When a match is created, the supervisor establishes the match identity, game configuration, map, participant roster, teams, and rules. It provisions one player runtime for each participating bot.

Each runtime connects through the ExampleBot/SC2 integration and confirms that it is associated with the expected in-game participant. Before accepting proposals, the host binds together:

- match identity;
- participant/player identity;
- bot implementation and version;
- runtime/session identity;
- available action vocabulary and policy configuration.

These are host-owned facts. CommandAi action and parameter IDs are not proof of player identity, ownership, or authorization. A proposal cannot make itself Player A by placing "Player A" in a string.

This becomes essential in team games and FFA. Two bots may be allies, but they remain distinct participants with independent state, command queues, and evidence.

### 3.2 Observation and context

ExampleBot receives observations from StarCraft II. The adapter translates raw game data into domain information the bot can consume. Depending on the API and configured visibility, observations may include own units and structures, resources and supply, visible enemies, known map locations, upgrades, health, energy, cooldowns, orders, game clock, race, and match metadata.

The bot should distinguish observed facts from estimates and strategic beliefs. An enemy base last seen several minutes ago is not equivalent to a currently visible structure.

The host decides what context is disclosed to the bot. CommandAi's address-space exchange model can be useful when an application needs declared, typed, direction-constrained context slots and correlated responses. Those slots are contracts, not pointers into live game state. The host retains ownership of the data and disclosure policy.

### 3.3 Decision and strategy

SingularityBot consumes observations through its FSM_API-driven state hierarchy. One possible decomposition is:

- **Match FSM:** opening, scouting, defense, attack, transition, recovery, and endgame.
- **Economy FSM:** worker production, resource allocation, expansion, gas timing, and supply management.
- **Army FSM:** composition, rally points, engagement selection, retreat, reinforcement, and upgrades.
- **Unit FSMs:** individual or grouped behavior such as worker assignment, Marine stutter-step, Zealot engagement, or Zergling surround.
- **Tactical FSMs:** short-lived behaviors such as focus fire, kiting, spell usage, and target reassessment.

This is an example, not a mandated tree. Other developers should be able to use different state machines, behavior trees, planners, search algorithms, learned policies, or hybrids.

The decision layer produces goals and tactical intent. For example: "Defend the natural expansion while the economy completes the next production milestone" or "Move this Marine group toward the rally point, but do not cross the threat boundary."

The first CommandAi integration should express a high-level order or bounded goal request. SingularityBot resolves that request into its own strategy descriptors, advisor goals, Task Master assignments, and FSM contexts. The existing FSMs then emit tactical intents for the current observation cycle. Those intents pass through SingularityBot's existing arbitration and validation pipeline before SC2 translation. CommandAi does not need to understand the strategic sentence, and it should not replace the bot's deterministic tactical controllers.

### 3.4 Proposal through CommandAi

The current CommandAi/1 proposal model is generic: an immutable ordered collection of commands, each with an opaque positive action ID and typed arguments. Integer identifiers and literal text are values; their meanings come from the consuming application's trusted vocabulary.

For SingularityBot, the preferred initial vocabulary should describe orders and goal-level requests rather than raw SC2 ability calls. Examples might include a strategic posture request, an expansion objective, a scouting assignment, a force-assembly goal, or a production/composition objective. The bot's trusted resolver maps those requests into its own strategy and goal model. The resulting FSM execution still flows through the current intent arbitration, live-state validation, and SC2 action translation path.

This preserves a useful separation: CommandAi can provide a common external order interface for different bot implementations, while SingularityBot retains its internal strategy and tactical behavior. A future consumer may also use CommandAi for lower-level atomic actions, but that is a separate adapter contract and should not bypass the same validation boundary.

A bot-specific adapter could map intents into actions such as:

| Domain intent | Illustrative host vocabulary | Illustrative arguments |
|---|---|---|
| Move a unit or group | Move | unit/group reference, world position |
| Attack a target | Attack | unit/group reference, target reference |
| Gather resources | Gather | worker reference, resource-node reference |
| Train a unit | Train | production-structure reference, unit type |
| Build a structure | Build | worker reference, structure type, placement |
| Research an upgrade | Research | structure reference, upgrade identity |
| Use an ability | UseAbility | unit reference, ability identity, optional target |

These names are explanatory examples, not a built-in CommandAi action catalog or a claim that these exact IDs exist. The consuming bot/host integration owns the vocabulary, schemas, and mappings.

A Marine kiting decision might produce an ordered proposal containing an attack intent and a movement intent. The adapter must define whether that pair is valid for the current SC2 API frame, whether the commands are compatible, and what order means. CommandAi preserves proposal order; it does not promise transactional execution or infer dependencies.

The current /1 contract does not itself provide a StarCraft action schema, live unit references, a precondition engine, idempotent execution, an execution receipt, or authorization. Those belong to the integration and host. A well-formed proposal is not an executable or authorized game action.

### 3.5 Validation and authorization

The trusted host receives a proposal and validates it against the live game before any side effect.

A robust validation path asks:

1. Is the proposal structurally valid under the supported CommandAi contract and resource limits?
2. Is the sender bound to the player runtime that submitted it?
3. Does each action ID resolve to a known action in the trusted vocabulary?
4. Are argument IDs and value kinds correct for that action?
5. Does each referenced unit belong to this player and still exist?
6. Is the target valid and visible/known under the applicable game rules?
7. Is the requested ability available to this unit in its current state?
8. Are placement, range, cooldown, energy, resources, supply, and other live preconditions satisfied?
9. Does the request comply with match policy, rate limits, and configured approval/safety rules?
10. Can the adapter translate the accepted intent into a legal SC2 action for the current frame?

A failure should produce a rejection or a clearly classified non-execution result. The host must not silently reinterpret malformed or unauthorized requests as a different action.

The host may normalize explicitly documented details, such as quantizing a coordinate to the game's supported representation. Such normalization must be part of the trusted adapter contract and visible in evidence. It must not conceal a strategy change.

### 3.6 Dispatch through the SC2 adapter

Only after validation does the current SingularityBot action pipeline translate accepted tactical intents to the ExampleBot/SC2 adapter. In the proposed high-level CommandAi integration, the goal resolver is not itself an SC2 dispatcher: it creates or updates bot-owned goals, and the existing action pipeline remains responsible for live-state validation and submission.

Game-specific details belong here: mapping domain actions to SC2 abilities, translating unit and target references, converting positions, batching compatible actions for a frame, handling API errors, and respecting per-frame and per-unit constraints.

The adapter should be deliberately boring: translate approved commands, report what the game API accepted or rejected, and avoid making strategic decisions that belong to the bot.

If the adapter must make a tactical choice to satisfy an API constraint, that choice should be explicit, bounded, and observable. Otherwise, the action the bot intended may differ invisibly from the action the game received.

### 3.7 Feedback and observed outcomes

A submitted action is not the same as a successful game outcome.

The adapter reports dispatch information, and subsequent observations reveal what actually happened. A unit may have been destroyed, a target may have moved, a structure may have completed, or the game may have rejected an action. The bot updates its FSM contexts from observed state rather than assuming every proposal succeeded.

The conceptual lifecycle should distinguish:

- **Proposed:** what the bot asked to happen.
- **Accepted/rejected:** what the trusted host decided.
- **Dispatched:** what the adapter submitted to the game API.
- **Observed:** what subsequent game observations show.
- **Attributed:** which match, player, bot version, and decision cycle the evidence belongs to.

These are distinct facts. The current CommandAi proposal is not an execution receipt or outcome protocol. The integration must use host/runtime records and the appropriate broader Workshop contracts for correlation and provenance.

## 4. Concrete example: Marine stutter-step

Marine stutter-step is a useful first integration proof because it exercises repeated decisions, unit identity, target identity, movement, attack timing, range, cooldowns, and feedback.

1. **Observe.** The bot sees its Marines, their positions and weapon state, and an enemy unit that is currently visible.
2. **Assess.** The tactical FSM determines that the Marines can attack, but should create distance before the enemy can retaliate effectively.
3. **Choose.** The unit FSM selects a target and a safe movement direction from the current observation.
4. **Propose.** The bot adapter creates a bounded CommandAi proposal representing the intended attack/movement behavior using the host's trusted vocabulary.
5. **Validate.** The host confirms the Marines belong to this player, the target is valid, the ability is available, the values are well-formed, and the requested actions are compatible with the current frame.
6. **Dispatch.** The SC2 adapter submits the accepted actions.
7. **Observe again.** The next observation shows actual positions, orders, damage, cooldowns, and units lost or gained.
8. **Correct.** The FSM updates its state and decides whether to attack, retreat, retarget, regroup, or continue kiting.

The loop repeats. It does not rely on a long-running instruction such as "stutter-step forever." It repeatedly makes bounded decisions against fresh state.

This gives the system a clean place to handle stale targets, dead units, lost visibility, cooldown changes, pathing obstruction, and rejected actions. The bot owns tactical intent; the host owns whether the request can be carried out now.

## 5. Scaling to multiplayer

The command contract should not change because a match has more participants. What changes is match configuration, player identity, team relationships, and supervisor orchestration.

| Match format | Runtime arrangement | Important isolation rule |
|---|---|---|
| 1v1 | Two independent player runtimes | Each runtime can command only its own player |
| 2v2 | Four independent runtimes grouped into two teams | Teammate relationship does not merge player authority |
| 3v3 | Six independent runtimes | Team coordination is explicit; each bot retains its own identity |
| FFA | One runtime per participant | No implicit ally or shared-state assumption |
| 2v2v2 | Six independent runtimes across three teams | Team membership is metadata, not an authorization shortcut |
| Other team formats | One runtime per participating bot | Participant count and team topology are configuration |

The supervisor models participants and teams as match data rather than hard-code two sides. A match may have no teams, two teams, or several teams. Alliances may be static or governed by a particular mode.

### Team coordination

Team bots may coordinate scouting, attack timing, target selection, defense, and resource strategy. Coordination should be an explicit capability.

A team service might exchange bounded strategic messages such as "I am expanding north" or "attack timing proposed at game clock X." It authenticates the sender, enforces what information teammates may share, and keeps coordination messages distinct from commands that cause game actions.

A teammate's recommendation is not permission to control another player's units. Each player runtime remains responsible for its own proposals and execution authority unless a game mode explicitly defines a shared controller.

## 6. An ecosystem for independent bot developers

The ladder should not require every developer to use SingularityBot's internal FSM architecture.

A developer could build a bot using SingularityBot/FSM_Bot, a different state-machine library, a behavior tree, a planner, search, a machine-learning policy, or a hybrid of deterministic rules and learned decisions.

The developer implements the agreed player-runtime integration and emits compatible proposals using the supported command vocabulary. The trusted host resolves and validates those proposals exactly as it would for a first-party bot.

This creates a useful separation:

- **Bot strategy is replaceable.**
- **The command boundary is shared.**
- **The game adapter and host remain trusted.**
- **The match supervisor treats each participant consistently.**

A developer kit should eventually include the command vocabulary and schemas, example adapters, a local mock host, observation fixtures, deterministic test scenarios, match configuration examples, and conformance tests. These are future deliverables, not current CommandAi features.

## 7. Evidence and observability

A serious ladder needs more than a win/loss result. It needs enough evidence to explain how a bot behaved and whether the runtime treated participants consistently.

A decision trace should be attributable to:

- match and game identifiers;
- participant and team identifiers;
- bot name, version, and configuration;
- observation/decision cycle or game frame;
- proposal identity in the host's correlation system;
- proposal content or a privacy-safe reference to it;
- validation result and rejection reason, if any;
- dispatch result from the SC2 adapter;
- subsequent observed state relevant to the action;
- runtime errors, timeouts, and disconnects.

The trace preserves the difference between a bot request and a game outcome. It should make it possible to reconstruct the relevant sequence without relying on mutable in-memory state after the match ends.

For competitive fairness, the supervisor should record match configuration, map, game version, timing rules, participant versions, and policy decisions that could affect execution. Evidence collection should be consistent across all players.

CommandAi provides the proposal representation. It does not define this complete telemetry or match-evidence model.

## 8. Failure handling and fairness

| Failure | Expected owner and behavior |
|---|---|
| Bot crashes or stops responding | Player runtime/supervisor detects timeout and applies published match policy |
| Malformed proposal | Decoder or host rejects it; no action is dispatched |
| Unknown action or parameter ID | Trusted vocabulary resolver rejects it |
| Unit or target no longer exists | Live-state validation rejects or classifies it as stale |
| Action unavailable this frame | Host/adapter reports non-dispatch; bot reevaluates from new observations |
| SC2 API error | Adapter reports failure; bot does not assume success |
| Player references another player's unit | Host ownership validation rejects it |
| Teammate disconnects | Supervisor applies explicit match rules; authority is not silently transferred |
| Excessive proposal rate or size | Host enforces configured limits and records the policy decision |

Fairness requires the same published rules for every participant. Rate limits, timeouts, observation disclosure, action scheduling, and disconnect behavior should be mode configuration—not ad hoc exceptions.

A rejected command should not automatically forfeit a match unless the published rules say so. Equally, a bot must not gain an advantage by flooding the host or exploiting ambiguous action scheduling.

## 9. Why CommandAi should remain small

It may be tempting to add StarCraft unit types, ability catalogs, map coordinates, player identities, cooldown fields, and execution receipts directly to CommandAi. That would make the package less useful.

Those concepts have different owners:

- StarCraft-specific action meaning belongs to the bot/SC2 integration vocabulary.
- Current unit ownership and visibility belong to the live game host.
- Match and participant identity belong to the supervisor/runtime.
- Strategy belongs to SingularityBot or another bot implementation.
- Authorization and dispatch belong to the trusted host.
- Generic proposal representation belongs to CommandAi.

The current CommandAi/1 contract is intentionally small and provider-neutral. A consuming domain can define a typed vocabulary and adapter without teaching the generic package what a Marine or a Nexus is.

SingularityBot already has a more specific internal domain model: strategy/advisor assessments, Task Master assignments, persistent entity contexts, FSM goals, tactical intents, and an SC2 action pipeline. CommandAi should bridge into the appropriate goal/order boundary, not duplicate those models inside a second command engine.

GrammarAi can be used where a structured grammar is genuinely needed, but grammar validation is not authorization. The current CommandAi/1 proposal model should not be described as already carrying a StarCraft-specific grammar payload.

## 10. Staged implementation path

The architecture should be proved in small steps.

### Stage 1 — Define the bot/host vocabulary

Specify the first atomic actions and parameter schemas required by one Marine scenario. Define identifier ownership, value kinds, target representation, position representation, and error categories. Keep the vocabulary in the StarCraft integration, not CommandAi.

**Exit evidence:** documented schema, positive/negative tests, and a resolver that rejects unknown IDs and malformed arguments.

### Stage 2 — Build a proposal adapter without live execution

Translate one deterministic SingularityBot behavior intent into a CommandAi proposal. Decode it on the host side and inspect the resolved domain operation. Do not yet submit actions to StarCraft.

**Exit evidence:** repeatable intent-to-proposal-to-resolved-operation tests, including invalid and stale references.

### Stage 3 — Add live-state validation

Connect the resolver to a controlled SC2 observation. Validate player ownership, unit existence, target visibility, ability availability, and frame-specific constraints.

**Exit evidence:** valid requests pass; stale, cross-player, unavailable, and malformed requests fail closed.

### Stage 4 — Execute one bounded action

Dispatch one approved movement or attack action through the ExampleBot adapter. Capture the adapter response and verify the result through subsequent observations.

**Exit evidence:** an end-to-end trace distinguishes proposed, accepted, dispatched, and observed states.

### Stage 5 — Prove Marine stutter-step

Add repeated decisions, movement/attack compatibility, target changes, unit loss, and cooldown changes. Test the feedback loop under deterministic scenarios before broad live matches.

**Exit evidence:** reproducible scenarios show that the bot reevaluates from observations rather than assuming action success.

### Stage 6 — Prove player isolation and team modes

Run multiple independent player runtimes. Prove that a participant cannot submit actions for another participant, team metadata does not merge authority, and FFA and multi-team configurations are data-driven.

**Exit evidence:** adversarial cross-player tests and repeatable 1v1, 2v2, FFA, and 2v2v2 supervision tests.

### Stage 7 — Open the developer ecosystem

Publish the runtime integration contract, conformance suite, sample bot, local test harness, and match submission requirements. Admit external bots only after they pass the same conformance and isolation checks.

**Exit evidence:** an independently implemented bot participates without requiring the supervisor to understand its internal strategy.

CiAi can make these gates repeatable: build affected repositories, run contract and adapter tests, retain exact commit/test evidence, and reject incomplete integration claims. A green unit-test suite alone is not proof of a working live match.

## 11. Current scope versus proposed integration

This guide does not claim that:

- CommandAi contains a StarCraft action vocabulary;
- CommandAi directly references FSM_API, SingularityBot, or ExampleBot;
- the CommandAi codec validates live game state or authorizes actions;
- a complete CommandAi-to-SC2 adapter already exists;
- the multiplayer supervisor, ladder, team coordination, or external developer onboarding is complete;
- proposal order guarantees atomic execution or all-or-nothing game effects.

The current CommandAi/1 model provides a generic representation boundary. A SingularityBot goal/order vocabulary and resolver, any shared host integration, multi-player supervisor, and cross-player isolation tests must be implemented and verified in their owning repositories. SingularityBot's existing tactical intent validation and SC2 action pipeline remain the live execution authority. FSM_Foundry is an intended shared executor but is not yet referenced by the current SingularityBot project; documentation must not imply otherwise.

## 12. The architectural payoff

The end state is a platform in which strategy can be ambitious without making execution authority ambiguous.

SingularityBot can evolve from a Marine-focused bot into a collection of hierarchical, configurable strategies. Other developers can bring different decision engines. A supervisor can run two players, six players, or more, with arbitrary team topologies. The same host boundary can validate every participant's actions. Observations close the loop, and evidence makes behavior inspectable.

CommandAi contributes one deliberately narrow but valuable piece: a common way to represent proposed actions without confusing a proposal with permission.

**The bot owns the decision. The host owns the authority. The game owns the observed outcome.**

## Related documentation

- [CommandAi architecture](ARCHITECTURE.md)
- [CommandAi theory and wire contract](THEORY.md)
- [CommandAi usage](USAGE.md)
- [Domain completeness and ownership](DOMAIN_COMPLETENESS.md)
- [Workshop AI package series](AI_SERIES.md)
- [SingularityBot repository](https://github.com/TrentBest/TheSingularityWorkshop.SingularityBot)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

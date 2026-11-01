<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Command domain completeness and ownership

> Status: source-reviewed on the current `development` branch. This is an architectural inventory, not a claim that every candidate capability is implemented.

## 1. What a command means

A **command** is a bounded declaration of an intended atomic action against a target, within a scope, using explicitly identified parameters, subject to declared preconditions, with a declared shape for the expected outcome.

A command is not:
- an authorization token;
- proof that its preconditions are true;
- a promise that the action will execute;
- an execution result;
- a transaction spanning several commands;
- a workflow, schedule, retry policy, or compensation plan;
- a screen, user interaction, lesson, narration, animation, or haptic signal.

CommandAi represents an untrusted proposal. A trusted host must independently interpret the application-owned identities, validate live state, apply policy, authorize, decide whether approval is required, dispatch, execute, and correlate actual outcomes.

## 2. The semantic kernel and its projections

CommandAi describes **what is being requested**, not how a person or agent must experience it.

The same semantic action may be projected differently by future application layers:
- an early learner may see a shape, hear a spoken word, and see the matching letters;
- a more experienced learner may see a sentence, choose among actions, and receive richer feedback;
- an expert may see structured parameters, preconditions, diagnostics, and an advanced editor;
- an AI observer may receive a structured contract with stable identities and explicit capability boundaries.

These are different observer-facing presentations of the same domain meaning. They must not require different command semantics merely because the user is a different age, has a different reading level, prefers another modality, or needs different accessibility support.

Therefore CommandAi must not acquire dependencies on GUI, GUI.SVG, Visualize, speech, haptics, pedagogy, user profiles, developmental stages, or AppAi. Presentation systems may interpret the same command through their own capabilities and policies. A proposal's existence never grants the observer permission to perform it.

## 3. Current ownership map

| Concern | Current evidence | Owner and disposition |
|---|---|---|
| Protocol version | `CommandProposal.CurrentProtocolVersion` is `CommandAi/2`; decoder rejects other versions | **CommandAi — implemented** |
| Command identity | Required non-empty `Guid` per command; duplicate identities rejected within a proposal | **CommandAi — implemented** |
| Action identity | Required positive opaque `ulong` | **CommandAi — implemented** |
| Target and scope | Required typed opaque `CommandValue` values | **CommandAi — implemented** |
| Parameters | Ordered `CommandArgument` collection; positive unique parameter identities; integer-identifier or literal-text values | **CommandAi — implemented, intentionally minimal** |
| Parameter schema, optionality, defaults, constraints | No schema/version, optional/default metadata, or parameter validation contract in the current model | **Gap — evaluate only with concrete generic consumer need** |
| Preconditions | Grammar version and expression are preserved; CommandAi does not parse or evaluate them | **CommandAi stores; GrammarAi defines grammar; host evaluates against live state** |
| Idempotency | Required bounded string; duplicate keys rejected within one proposal | **CommandAi stores identity; host defines scope, persistence, replay detection, and enforcement** |
| Expected outcome | Outcome identity and typed required/optional field declarations | **CommandAi — implemented as a declaration only** |
| Actual result, error, correlation | Not represented by the expected-outcome contract | **ProtocolAi and host — actual outcome/event lifecycle** |
| Proposal identity and causal/correlation identity | No distinct proposal ID, parent ID, correlation ID, or causation ID in the current public model | **Gap — coordinate with ProtocolAi before changing the wire contract** |
| Multi-command order | Proposal preserves an ordered list of commands | **CommandAi — implemented as representation; no transaction or dependency semantics** |
| Dependencies, branching, workflow | Not modeled or executed | **Composition/workflow owner (e.g. CiAi or AppAi); keep outside atomic command kernel** |
| Cancellation, expiry, deadlines, retry, compensation | Not modeled | **Host/orchestrator policy; do not imply these by adding fields without lifecycle ownership** |
| Resource bounds | Limits exist for envelope, commands, arguments, preconditions, outcome fields, literals, grammar version, expression, idempotency key, and aggregate literals | **CommandAi — implemented representation safeguards** |
| Encoding and decoding | Line-oriented CommandAi/2 envelope, strict JSON member handling, depth and size bounds, typed reconstruction | **CommandAi — implemented** |
| Canonical bytes / signatures / hashes | No canonical cross-platform byte contract, signing, or digest API is declared | **Not implemented; do not use encoded text as a signature format** |
| Unknown extensions | Unknown envelope entries and JSON members are rejected | **Strict current contract; extension mechanism is not implemented** |
| Secret classification and redaction | Values are opaque but have no secret marker, secure storage, or redaction policy | **Host/security layer; opaque does not mean secret-safe** |
| Authorization and execution | No policy engine, credentials, dispatch, or execution API | **Foundry/host — intentionally excluded** |
| Presentation, narration, haptics, accessibility, pedagogy | No presentation model | **GUI/Renderer/Visualize/AppAi/experience owner — intentionally excluded** |

## 4. What the current implementation guarantees

The public model and tests currently establish these representation-level guarantees:

- command identity, action identity, target, scope, idempotency key, and expected outcome contract are required;
- argument and outcome-field identities are positive and unique within their owning collections;
- command and proposal collections preserve insertion order;
- a proposal is non-empty and bounded;
- duplicate command identities and duplicate idempotency keys within one proposal are rejected;
- preconditions retain their declared grammar version and expression without evaluation;
- typed values preserve the distinction between integer identifiers and literal text;
- malformed envelopes, unsupported protocol versions, duplicate protocol declarations, unknown entries, unknown JSON properties, invalid typed values, and oversized input are rejected;
- builders produce immutable snapshots; builders themselves are mutable and not thread-safe.

These guarantees do **not** establish semantic correctness of action IDs, target IDs, scopes, parameters, or expressions. Nor do they establish idempotent execution, safe authorization, successful execution, or a valid CI verdict.

## 5. Composition boundary

A proposal may carry several ordered atomic commands. This is a useful transport shape, but it is not a workflow language.

A higher-level composer may build a sequence or graph of command proposals and validate its structure with GrammarAi. ProtocolAi may carry correlated handoff and result envelopes. CiAi may contribute repository-development vocabulary and pure authoring helpers. Foundry and the host must independently resolve capabilities, inspect live state, authorize each action, schedule it, execute it, and record actual outcomes.

A consumer must not infer that:
- order alone implies a dependency;
- a later command is conditional on an earlier command succeeding;
- the entire proposal commits or rolls back atomically;
- a repeated idempotency key is globally safe;
- a declared expected outcome will necessarily occur.

## 6. Compatibility and extension policy

CommandAi/2 is the only currently supported wire version. The decoder is strict: unknown fields are rejected rather than silently ignored. This avoids pretending that an older reader understands data it does not.

Before adding proposal identity, parameter schemas, extension bags, causal links, or other cross-cutting fields, coordinate ownership with ProtocolAi and the host. A wire change must specify:
1. the semantic owner of each field;
2. whether it is required, optional, or version-gated;
3. validation and resource bounds;
4. unknown-field behavior;
5. deterministic serialization expectations;
6. migration behavior for alpha consumers;
7. tests proving both accepted and rejected cases.

Alpha consumers may require source updates for breaking changes. The migration guide must identify the exact change and replacement pattern. Do not preserve a misleading compatibility promise merely to avoid a documented alpha migration.

## 7. Visual documentation

Visuals should expose the domain, not prescribe a user interface. A command lifecycle diagram should show the proposal boundary, grammar interpretation, host validation and authorization, execution, and correlated outcome as distinct responsibilities. A type diagram should show containment and typed identity without implying that identifiers are globally resolvable.

Use GUI.SVG only after its public API and intended documentation use are verified. Until then, keep diagrams as accessible, version-controlled SVG assets with equivalent explanatory Markdown. Avoid introducing a runtime package dependency solely to render repository documentation.

## 8. Release-readiness evidence still required

- [ ] Complete concept-to-owner gap review, including ProtocolAi and Foundry alignment.
- [ ] Decide whether proposal/correlation identity belongs here or in ProtocolAi.
- [ ] Decide whether generic parameter schema metadata is justified by multiple consumers.
- [ ] Add adversarial codec tests for all required-field/type combinations and resource-boundary edges.
- [ ] Verify deterministic output across supported environments before claiming canonical encoding.
- [ ] Generate and inspect human-readable domain diagrams with textual equivalents.
- [ ] Validate every Markdown page has the animated moniker and canonical PNG footer in CI.
- [ ] Verify build, tests, quickstart, packed-package audit, and packed-consumer compilation on the exact release candidate.
- [ ] Review NuGet metadata, package contents, license, README, XML docs, and dependency-free core.
- [ ] Obtain explicit owner authorization before any NuGet publication.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

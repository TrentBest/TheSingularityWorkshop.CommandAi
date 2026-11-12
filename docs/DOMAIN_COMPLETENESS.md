<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi domain completeness and ownership

> Status: compact and enriched atomic proposal forms plus the address-space exchange/response JSON codec are represented under CommandAi/1. The protocol identity remains CommandAi/1; internal evolution does not itself require a protocol or NuGet version increment.

## 1. Domain definition

CommandAi represents two related but distinct data contracts:

- **Atomic proposals** describe bounded action intent using compact legacy commands or enriched atomic-command declarations.
- **Address-space exchanges** describe named, typed, direction-constrained slots through which an application supplies context and receives proposed values. A local exchange identifier correlates request and response.

The application retains ownership of actual memory, persistence, calculations, operation semantics, and execution. An address is a contract identifier, never a pointer or a direct memory capability. The exchange model adds structured context and response values without replacing the atomic proposal model.

## 2. Atomic proposal contract

| Concern | Current implementation | Owner / boundary |
|---|---|---|
| Protocol identity | CommandProposal.CurrentProtocolVersion is CommandAi/1 | CommandAi |
| Proposal structure | Non-empty ordered collection; compact and enriched entries may coexist | CommandAi |
| Command identity | Non-empty GUID, distinct from action identity; unique among enriched commands in one proposal | CommandAi carries identity; host owns lifecycle/correlation |
| Action identity | Positive opaque integer | CommandAi validates representation; consuming host defines meaning |
| Target and scope | Required typed opaque values for enriched commands; absent for compact commands | CommandAi bounds representation; consuming host defines meaning and validates current state |
| Parameter identity/value | Positive parameter ID paired with integer identifier or bounded literal text; duplicate parameter IDs rejected per command | CommandAi |
| Preconditions | Optional bounded expression paired with a grammar-version identifier | CommandAi carries opaque data; GrammarAi owns grammar/structural validation; host evaluates live state |
| Idempotency identity | Required bounded string for enriched commands; unique among enriched commands in a proposal | CommandAi carries identity only; host owns replay/deduplication; no exactly-once guarantee |
| Expected outcome | Required outcome identity and bounded unique field declarations for enriched commands | CommandAi declares shape only; ProtocolAi/runtime carries actual correlated outcomes |
| Actual result and errors | Not represented as command intent | ProtocolAi and runtime/host |
| Ordering | Proposal preserves insertion order | Representation only; no dependency semantics |
| Workflow/transaction/retry/cancellation/compensation | Not represented or executed | Orchestrator / host |
| Resource bounds | Envelope 262,144; commands 256; arguments/command 64; preconditions/command 32; outcome fields 64; literal 16,384; grammar version 128; precondition expression 16,384; idempotency identity 256; aggregate in-memory literal data 1,048,576; enriched JSON depth 16 | CommandAi representation safeguards |
| Encoding and decoding | Legacy Command line unchanged; enriched AtomicCommand JSON line; mixed proposals supported; strict duplicate/unknown JSON-property rejection | CommandAi codec |
| Older-reader compatibility | Older compact-only /1 readers reject AtomicCommand entries | Consumer upgrade required before enriched forms are emitted/accepted; not universal forward compatibility |
| Canonical bytes, signatures, hashes | No canonical signing/digest contract | Not implemented |
| Secret classification and redaction | No secret metadata or redaction API | Host/security layer; opaque does not mean secret-safe |
| Authorization and execution | No policy, credential, dispatch, or execution API | FSM_Foundry / host |

These are representation guarantees. They do not establish that an ID exists in a host vocabulary, that a precondition is true, that an idempotency key has been enforced, or that execution is safe or authorized.

## 3. Address-space exchange guarantees

| Concern | Current implementation |
|---|---|
| Address identity | Positive application-defined integer ID |
| Address metadata | Name, optional description, access direction, declared value kind |
| Direction | Read, write, or read/write; request values require read access and response values require write access |
| Required slots | Required input and required output flags enforced independently |
| Typed values | Existing integer-identifier and literal-text kinds; value kind must match descriptor |
| Grammar binding | Optional positive GrammarAi definition ID + schema version pair; metadata is opaque here |
| Request/response correlation | Non-empty GUID; response validation requires matching GUID and equivalent address-space descriptors |
| Duplicate/unknown slots | Rejected |
| Immutability | Descriptor and value objects are immutable; collections are defensive read-only snapshots |
| Resource bounds | At most 1,024 descriptors and values; literals at most 16,384 UTF-16 characters; encoded exchange/response envelope at most 262,144 characters |
| Wire format | Deterministic compact JSON; strict protocol/kind, field, duplicate-property, type, and structural validation |

## 4. Explicit ownership

- **CommandAi:** immutable atomic proposal data, exchange descriptors and values, direction/type-shape checks, required-slot checks, local exchange correlation, bounded collections, and proposal/exchange codecs.
- **GrammarAi:** grammar definitions and structural validation of grammar-governed content; it does not establish live-state truth.
- **ProtocolAi:** shared vocabulary identity and broader causal/event/outcome provenance.
- **CiAi / consuming application:** domain-specific command/action definitions and mappings to grammar definitions and protocol categories.
- **HeadlessAi:** configured model endpoint communication.
- **AppAi (future):** composition of application programs, chaining, branching, and execution-plan representation.
- **FSM_Foundry / host:** resolve identities against trusted capabilities and live state; policy, authorization, approval, replay/delivery, scheduling, dispatch, execution, lifecycle, and persistence.

## 5. Deliberate exclusions

CommandAi does not dereference application addresses, read/write live memory, evaluate business semantics or preconditions, resolve capability names, authorize, execute, persist, transport to endpoints, or orchestrate workflows. It does not guarantee retries, cancellation, idempotent handling, atomicity, or exactly-once delivery.

A grammar-valid payload remains untrusted intent. GrammarAi validation is not host authorization. An expected-outcome contract is a declaration, not an actual result, success signal, CI verdict, or proof of execution.

## 6. Wire contract, compatibility, and scale

The original compact Command= proposal line remains byte-for-byte stable. Enriched commands use an explicit AtomicCommand= JSON entry under the same CommandAi/1 identifier. New readers accept compact, enriched, and mixed proposals. Older compact-only readers reject the enriched entry and must be upgraded before consuming it; retaining the /1 label does not make old readers forward-compatible.

The exchange/response JSON envelope uses fixed canonical field order and exact string names for access and value kinds. Decoding rejects unknown and duplicate properties, malformed or empty exchange IDs, invalid descriptor/value data, and payloads above the 262,144-character envelope bound. The codec enforces a maximum JSON nesting depth of 16 and does not permit comments or trailing commas.

Before accepting high-volume or 10,000-command scenarios, define separate framing/streaming, chunking, ordering, cancellation, idempotency, and partial-failure semantics. Neither codec is a multi-command execution protocol.

## 7. Version policy

CommandAi/1 is the protocol identity used for internal Workshop coordination and may evolve in place by owner direction. The NuGet package version is a separate release label. No package version bump, publication, merge, release, or deployment is implied by these source changes.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
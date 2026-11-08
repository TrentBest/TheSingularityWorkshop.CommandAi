<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi domain completeness and ownership

> Status: this document distinguishes implemented in-memory models from wire-format work still outstanding. The protocol identity remains CommandAi/1; internal evolution does not itself require a protocol or NuGet version increment.

## 1. Domain definition

CommandAi represents an exchange between an application and an AI-facing consumer. The application defines an address space: named, typed, direction-constrained slots through which it supplies context and receives proposed values. A local exchange identifier correlates request and response.

The application retains ownership of actual memory, persistence, calculations, operation semantics, and execution. An address is a contract identifier, never a pointer or a direct memory capability.

The existing atomic proposal model remains available for compact action/argument interchange. It is not the limit of the domain.

## 2. Implemented exchange guarantees

| Concern | Current implementation |
|---|---|
| Address identity | Positive application-defined integer ID |
| Address metadata | Name, optional description, access direction, declared value kind |
| Direction | Read, write, or read/write; request values require read access and response values require write access |
| Required slots | Required input and required output flags enforced independently |
| Typed values | Existing integer-identifier and literal-text kinds; value kind must match descriptor |
| Grammar binding | Optional positive GrammarAi definition ID + schema version pair; metadata is opaque here |
| Request/response correlation | Non-empty GUID; response validation requires matching GUID and same address-space instance |
| Duplicate/unknown slots | Rejected |
| Immutability | Descriptor and value objects are immutable; collections are defensive read-only snapshots |
| Resource bounds | At most 1,024 descriptors and 1,024 values per exchange/response; existing literal text bound remains 16,384 UTF-16 characters |

## 3. Explicit ownership

- **CommandAi:** generic descriptors, direction, type-shape agreement, required-slot checks, local exchange correlation, and bounded in-memory collections.
- **GrammarAi:** grammar definition registry and structural validation of grammar-governed bodies.
- **ProtocolAi:** shared vocabulary identity and broader causal/event/outcome provenance.
- **CiAi / consuming application:** domain-specific command/action definitions and mappings to grammar definitions and protocol categories.
- **HeadlessAi:** configured model endpoint communication.
- **AppAi (future):** composition of application programs, chaining, branching, and execution-plan representation.
- **FSM_Foundry / host:** resolve address meaning against trusted capabilities and live state; policy, authorization, approval, scheduling, dispatch, execution, lifecycle, persistence.

## 4. Deliberate exclusions

CommandAi does not dereference application addresses, read/write live memory, evaluate business semantics, resolve capability names, authorize, execute, persist, transport to endpoints, orchestrate workflows, or guarantee retries, cancellation, idempotency, atomicity, or exactly-once delivery.

A grammar-valid payload remains untrusted intent. GrammarAi validation is not host authorization.

## 5. Wire-format status and scale

The existing compact proposal text codec remains intact. A canonical bounded serialization format for the new exchange and response objects is not yet implemented; consumers must not assume the object model is already wire-ready. Before accepting high-volume or 10,000-command scenarios, define framing, maximum encoded bytes, streaming/chunking, ordering, cancellation, idempotency, and partial-failure semantics.

## 6. Version policy

CommandAi/1 is the protocol identity used for internal Workshop coordination and may evolve in place by owner direction. The NuGet package version is a separate release label. No package version bump, publication, merge, release, or deployment is implied by these source changes.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

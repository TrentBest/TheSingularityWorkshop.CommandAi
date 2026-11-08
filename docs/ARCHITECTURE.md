<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi architecture

## Exchange model and wire representation

CommandAi has two representation surfaces within the same `CommandAi/1` protocol identity:

- The existing compact `CommandProposal` remains an ordered list of atomic action proposals.
- The address-space exchange model describes application-defined input/context slots and output slots, correlated by an exchange GUID.

`CommandExchangeTextCodec` serializes exchanges and responses as deterministic compact JSON. The envelope declares the protocol, message kind, exchange ID, complete address descriptors, and the corresponding input or output values. Property order is fixed by the encoder; decoders reject duplicate or unknown properties, unsupported enum names, malformed identifiers, invalid descriptor/value shapes, and over-limit collections or envelopes. The compact proposal codec remains unchanged and line-oriented.

An address descriptor declares a positive local ID, name, access direction, value kind, optional description, required-input/output constraints, and optional GrammarAi definition identity plus schema version. Grammar metadata is opaque: CommandAi checks that both values are present and positive, but does not load GrammarAi, resolve definitions, or validate grammar payloads.

`CommandExchange` accepts values only for readable addresses and enforces required inputs. `CommandResponse` accepts values only for writable addresses and enforces required outputs. Both reject unknown or duplicate addresses and mismatched value kinds. `ValidateFor` verifies correlation and equivalent address-space descriptor snapshots.

## Responsibility boundaries

    Application / host
      owns address meaning, live memory, computation, persistence
                 |
                 v
       CommandExchange (readable context)
                 |
                 v
        LLM / model interaction
                 |
                 v
        CommandResponse (writable values)
                 |
                 v
    Host validates meaning, policy, authorization, then executes

The address space is a contract, not a memory map. Address IDs are not pointers. A structurally valid response is still untrusted intent. CommandAi never dereferences addresses, reads or writes application memory, authorizes, dispatches, or executes.

## Grammar-bound bodies

A writable address may identify a grammar-governed body through its GrammarAi definition ID and schema version. The body remains a bounded typed value at this layer. GrammarAi owns definition lookup and structural validation; CommandAi owns only generic address/value shape and correlation. There is no GrammarAi runtime dependency.

## Package ownership

- ProtocolAi owns shared vocabulary identities and broader causal/event/outcome provenance.
- GrammarAi owns grammar definitions and grammar-specific structural validation.
- HeadlessAi owns configured LLM endpoint communication.
- CiAi and consuming applications own their domain-specific definitions and mappings.
- AppAi may later compose multi-step application programs and response plans.
- FSM_Foundry / host owns live-state checks, policy, authorization, lifecycle, dispatch, and execution.

Exchange GUIDs correlate one local request and response; they are not a replacement for ProtocolAi's broader event/provenance model. CommandAi does not define workflows, branching, retries, cancellation, idempotent execution, or batch guarantees.

## Resource and trust boundaries

Address spaces and value collections are capped at 1,024 entries. Individual literal values retain the existing 16,384 UTF-16-character limit; encoded exchange/response envelopes are capped at 262,144 characters. These are structural bounds, not authorization limits or guarantees of safe execution. Large multi-command plans require a separate framing/streaming contract before any 10,000-command scale claim.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

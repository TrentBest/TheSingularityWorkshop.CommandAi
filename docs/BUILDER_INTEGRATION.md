<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi builder integration

> **Status: generic builder shipped; domain-specific extensions remain host-owned.** CommandAi supplies immutable proposal contracts, a deterministic text codec, and a small fluent builder for opaque action/parameter identities. It does not contain a command registry, plugin system, command executor, or host authorization mechanism.

## The layering decision

CommandAi should remain the small, reusable representation underneath a higher-level builder.

```text
Application / host
  policy, state, trust, authorization, approval, execution
        |
        v
CommandAi generic fluent builder
  opaque IDs, typed values, ordered proposal construction
        |
        v
Application/domain extensions
  named operations, typed domain validation, convenience methods
        |
        v
CommandAi proposal model
  CommandProposal
    -> CommandInstruction
       -> CommandArgument
          -> CommandValue
        |
        v
CommandAiTextCodec
  deterministic CommandAi/1 representation
```

The generic builder improves authoring ergonomics without assigning meaning to IDs. Application/domain extensions can layer named methods and semantic validation above it. CommandAi keeps the interchange contract stable and inspectable.

## Generic commands and extended commands

The generic layer should express only what can be represented without knowing the consuming application. `CommandProposalBuilder.AddCommand` creates ordered instructions; its scoped `CommandInstructionBuilder` supports `WithIdentifier`, `WithText`, and `WithValue`. `Build` returns an immutable proposal snapshot. The builder enforces the same argument and command limits as the proposal model. Proposal construction also enforces a 1,048,576-character aggregate literal-text budget across all commands.

The generic model supports:

- positive opaque action identifiers;
- positive opaque parameter identifiers;
- integer-identifier or literal-text values;
- ordered command proposals;
- versioned encoding and decoding.

An application or domain package can extend that foundation with named methods and typed input models. For example, a workshop-specific builder might expose `InspectResource(...)` or `OpenExperience(...)`, then translate those calls into the generic proposal model.

Those names and meanings belong to the application/domain layer. They must not become universal built-in meanings for numeric IDs in CommandAi.

## Example: host-owned named command

The following is consuming-application code, not a type shipped by CommandAi. The quickstart compiles this pattern in `examples/CommandAi.QuickStart/ResourceCommandExtensions.cs`; its numeric IDs are demonstration values owned by that example host:

```csharp
using TheSingularityWorkshop.CommandAi;

public static class ResourceCommandExtensions
{
    private const ulong InspectResourceAction = 1001;
    private const ulong ResourceIdParameter = 2001;

    public static CommandProposalBuilder InspectResource(
        this CommandProposalBuilder builder, ulong resourceId)
    {
        if (resourceId == 0)
            throw new ArgumentOutOfRangeException(nameof(resourceId));

        return builder.AddCommand(
            InspectResourceAction,
            command => command.WithIdentifier(ResourceIdParameter, resourceId));
    }
}

// The host now authors a named operation over the generic representation.
var proposal = new CommandProposalBuilder()
    .InspectResource(resourceId: 42)
    .Build();
```

The host extension owns the name `InspectResource`, the action/parameter identity mapping, and the domain-specific nonzero-resource check. CommandAi only stores the ordered identities and typed value. A different host can assign different IDs and expose different named methods without changing the generic package.

## What an extension may add

A higher-level builder may provide:

1. a trusted mapping from named operations to action and parameter identities;
2. typed convenience methods and domain-specific validation;
3. optional grammar-aware proposal construction, where the host has explicitly linked the grammar and protocol definitions;
4. diagnostics that explain why a proposal cannot be constructed;
5. conversion to `CommandProposal` for deterministic serialization.

A builder must not treat successful construction or encoding as authorization. The host still resolves identities against trusted definitions, checks current state and business rules, applies authorization and approval policy, and decides whether to execute.

## Extension boundary

Keep extension behavior outside the generic codec. The codec should not load application plugins, discover arbitrary command implementations, call services, or execute delegates while decoding untrusted input.

If reusable extension metadata is later required, design it as a separate, explicit contract with versioning and validation. Do not smuggle executable callbacks or host service references into the serialized proposal.

## Current implementation versus intended composition

| Capability | Current CommandAi source | Higher-level builder direction |
|---|---|---|
| Immutable proposal/instruction/argument/value contracts | Implemented | Consumed |
| CommandAi/1 text encode/decode | Implemented | Delegated to codec |
| Generic fluent proposal/instruction builder | Implemented | Used directly or wrapped |
| Application-specific command names | Not implemented | Host/domain extension |
| Protocol/grammar linking | Not implemented | Host-owned integration |
| Authorization, approval, execution | Explicitly outside scope | Host-owned |

The generic builder is implemented and tested as an authoring convenience. Domain-specific named commands and end-to-end integration remain host work. No compatibility or runtime behavior beyond the current source and tests is implied.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

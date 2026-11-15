<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAi package architecture

## 01 — What are the two packages?

CommandAi is distributed as a small core library and an optional MicroBundle adapter.

| Package | Owns | Dependencies |
|---|---|---|
| `TheSingularityWorkshop.CommandAi` | Immutable proposals, instructions, typed arguments, bounded CommandAi/1 text codec | None |
| `TheSingularityWorkshop.CommandAi.MicroBundle` | A code-defined MicroBundle descriptor/provider that exposes the core proposal codec | CommandAi core and MicroBundleDomain |

The core package remains useful in ordinary .NET applications. The adapter is installed only by consumers that want the CommandAi capability to participate in MicroBundle composition.

## 02 — Why is the adapter separate?

A consumer should not have to adopt a composition contract merely to encode or decode a proposal. Keeping the adapter optional preserves the core package's dependency-free boundary while still allowing the capability to be composed by any host that understands `IMicroBundle`.

The adapter depends on the neutral `TheSingularityWorkshop.MicroBundleDomain` contract. It does not depend on FSM_COS, MicroBundleRepository, a GUI framework, or a particular application host.

## 03 — How does the capability participate?

```text
Application / agent
       |
       v
CommandProposal
       |
       v
CommandAiTextCodec
       |
       v
bounded CommandAi/1 representation
       |
       v
trusted host resolves meaning, checks live state,
applies policy and authorization, then decides whether to execute

Optional composition path:

CommandAi.MicroBundle
       |
       +-- IMicroBundle
       +-- stable descriptor identity
       +-- commandai.proposal provider
       +-- Encode / Decode delegation
       +-- no dependencies of its own
       |
       v
MicroBundleDomain
       |
       v
MicroBundleRepository stores/retrieves the artifact
       |
       v
FSM_COS or another host composes the capability
```

`CommandAiMicroBundle` exposes the provider ID `commandai.proposal` and stable descriptor identity `0x434F4D4D414E4441`. Its descriptor capability version is independent of the NuGet package version and the `CommandAi/1` wire identity.

The adapter is stateless. `Load` validates that a host lifecycle context was supplied; it does not register host-specific services or execute proposals. `Arbitrate` reports no composition mutation because the codec has no cross-bundle arbitration behavior. The host can discover the capability through its descriptor/provider and use the concrete adapter's `Encode` and `Decode` methods.

## 04 — See it in a minute

Install the adapter package:

```bash
dotnet add package TheSingularityWorkshop.CommandAi.MicroBundle
```

A host-neutral consumer can then create the capability and use the canonical codec:

```csharp
var capability = new CommandAiMicroBundle();
var encoded = capability.Encode(proposal);
var decoded = capability.Decode(encoded);
```

The repository's package-only quickstart restores the adapter from the local packed-package feed, resolves its transitive CommandAi core and MicroBundleDomain dependencies, compiles, and runs a round-trip. That verifies package consumption; it does not prove that a particular FSM_COS composition manifest is wired to load it.

## 05 — Choose your next step

- [Package installation and consumer guide](../README.md#package-boundaries)
- [Problem domain](PROBLEM_DOMAIN.md)
- [Theory and wire contract](THEORY.md)
- [Governed command lifecycle](GOVERNED_COMMAND_LIFECYCLE.md)
- [CommandAiMicroBundle source](../microbundle/CommandAiMicroBundle.cs)
- [MicroBundleDomain contract](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
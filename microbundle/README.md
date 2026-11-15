<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ✳️ TheSingularityWorkshop.CommandAi.MicroBundle

[![Build](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml/badge.svg?branch=development)](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/workflows/build.yml)

## 01 What is this package?

This optional companion package exposes CommandAi's provider-neutral proposal codec as a code-defined MicroBundle. It implements `IMicroBundle` from `TheSingularityWorkshop.MicroBundleDomain` and declares the `commandai.proposal` provider.

Installing this package also brings in the CommandAi core and MicroBundleDomain dependencies. Applications that only need the CommandAi data model and codec should install `TheSingularityWorkshop.CommandAi` instead.

## 02 Why is it separate?

CommandAi remains independently useful outside a MicroBundle host. The adapter adds the composition contract only for consumers that want CommandAi to participate as a MicroBundle. It does not depend on FSM_COS, MicroBundleRepository, or a particular host.

## 03 How does it work?

- `CommandAiMicroBundle` exposes a stable descriptor and provider identity.
- `Encode` and `Decode` delegate to the canonical CommandAi/1 codec.
- `Load` participates in the host lifecycle without installing host-specific state.
- `Arbitrate` reports no composition mutation because this stateless codec has no cross-bundle arbitration behavior.

A successfully decoded proposal remains untrusted intent. The host still owns domain resolution, policy, authorization, approval, dispatch, and execution.

## 04 See it in a minute

```bash
dotnet add package TheSingularityWorkshop.CommandAi.MicroBundle --version 0.1.0-alpha.1
```

```csharp
using TheSingularityWorkshop.CommandAi;
using TheSingularityWorkshop.CommandAi.MicroBundle;

var capability = new CommandAiMicroBundle();
var proposal = new CommandProposal([new CommandInstruction(1001)]);
Console.WriteLine(capability.Descriptor.Providers[0].Id);

var decoded = capability.Decode(capability.Encode(proposal));
```

The package is validated through the repository's package-only consumer quickstart. That sample proves package restore, compilation, and codec round-trip; it does not execute a command.

## 05 Choose your next step

- [CommandAi core README](../README.md)
- [MicroBundleDomain 1.0.1](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain/1.0.1)
- [MicroBundleDomain architecture](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
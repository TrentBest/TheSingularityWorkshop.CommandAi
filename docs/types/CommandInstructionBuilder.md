<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandInstructionBuilder

## Purpose

Build one immutable atomic command with all required metadata.

## Contract

The constructor requires command identity, action identity, target, scope, idempotency key, and expected outcome contract. The fluent methods add typed arguments through WithIdentifier, WithText, and WithValue, and add opaque grammar-versioned preconditions through WithPrecondition. Build validates and snapshots the command.

The builder is mutable and not thread-safe. It performs representation validation only.

## Ownership

Source: [CommandInstructionBuilder.cs](../../CommandInstructionBuilder.cs). Related types: CommandInstruction, CommandArgument, CommandPrecondition, CommandOutcomeContract, CommandValue.

## Verification

[CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs) covers duplicate parameter rejection, metadata round-trip, precondition preservation, limits, and snapshot behavior.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

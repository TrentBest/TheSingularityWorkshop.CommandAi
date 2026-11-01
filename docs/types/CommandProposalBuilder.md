<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandProposalBuilder

## Purpose

Fluent, generic construction of an immutable ordered CommandProposal.

## Contract

AddCommand requires commandId, actionId, target, scope, idempotencyKey, and outcomeContract. Its optional configuration callback receives a CommandInstructionBuilder for ordered parameters and grammar-versioned preconditions. Build returns an immutable proposal snapshot and enforces the proposal command limit.

Application-specific names and semantic validation belong in host-owned extension methods, not in the generic builder.

## Ownership

Source: [CommandProposalBuilder.cs](../../CommandProposalBuilder.cs). Related type: [CommandInstructionBuilder](CommandInstructionBuilder.md).

The builder does not resolve identities, evaluate preconditions, enforce idempotency, authorize, or execute.

## Verification

[CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs) covers command ordering, builder limits, and independent immutable snapshots. [Quickstart](../../examples/CommandAi.QuickStart/Program.cs) demonstrates host-owned extensions.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

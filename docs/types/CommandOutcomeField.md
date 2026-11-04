<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandOutcomeField

An immutable declaration for one field in an expected command outcome contract.

## Contract

- FieldId is a positive application-owned identity.
- ValueKind is one of the supported CommandValueKind values.
- Required indicates whether the field must appear in a conforming outcome.

The field identity and value kind are opaque. They describe an expected shape, not an observed value or success verdict.

Source: [CommandOutcomeField.cs](../../CommandOutcomeField.cs). Related: CommandOutcomeContract, CommandValueKind.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

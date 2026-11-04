<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandPrecondition

An immutable, bounded opaque precondition expression paired with the grammar-version identifier needed to interpret it.

## Contract

- GrammarVersion is required and bounded to 128 UTF-16 characters.
- Expression is required and bounded to 16,384 UTF-16 characters.
- CommandAi preserves both values but does not parse, validate, or evaluate the expression.
- GrammarAi owns the grammar and structural validation.
- The host evaluates a validated condition against trusted current state.

A structurally valid precondition is not proof that the condition is true and does not authorize the command.

Source: [CommandPrecondition.cs](../../CommandPrecondition.cs). Related: CommandInstruction, GrammarAi.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

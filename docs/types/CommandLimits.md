<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandLimits

Central source constants that bound untrusted CommandAi/1 proposals and encoded envelopes.

| Bound | Maximum |
|---|---:|
| Encoded envelope | 262,144 UTF-16 characters |
| Commands per proposal | 256 |
| Arguments per command | 64 |
| Preconditions per command | 32 |
| Outcome fields per contract | 64 |
| Individual literal / target / scope value | 16,384 UTF-16 characters |
| Grammar-version identifier | 128 UTF-16 characters |
| Precondition expression | 16,384 UTF-16 characters |
| Idempotency identity | 256 UTF-16 characters |
| Aggregate in-memory literal data per proposal | 1,048,576 UTF-16 characters |
| Enriched JSON nesting depth | 16 |

The aggregate in-memory literal bound is intentionally independent of the smaller encoded envelope bound. A constructible proposal may still be too large to encode.

Source: [CommandLimits.cs](../../CommandLimits.cs). These are representation/resource limits, not semantic safety or authorization guarantees.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

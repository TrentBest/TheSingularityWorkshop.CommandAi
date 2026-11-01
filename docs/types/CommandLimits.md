<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandLimits

Central source constants that bound untrusted CommandAi/2 proposals and encoded envelopes.

| Bound | Maximum |
|---|---:|
| Encoded envelope | 262,144 characters |
| Commands per proposal | 256 |
| Arguments per command | 64 |
| Preconditions per command | 32 |
| Fields per outcome contract | 64 |
| Individual literal text | 16,384 characters |
| Aggregate literal, precondition, and idempotency text | 1,048,576 characters |
| Grammar-version identifier | 128 characters |
| Precondition expression | 16,384 characters |
| Idempotency key | 256 characters |

Source: [CommandLimits.cs](../../CommandLimits.cs). These are representation/resource limits, not semantic safety or authorization guarantees.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

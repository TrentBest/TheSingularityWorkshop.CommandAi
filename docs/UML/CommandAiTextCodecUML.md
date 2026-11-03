<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec — UML neighborhood and use cases

~~~mermaid
flowchart TD
  I[Untrusted CommandAi/1 envelope] --> C[CommandAiTextCodec]
  C --> P[CommandProposal]
  P --> A[Ordered CommandInstruction records]
  A --> M[Action identity and typed arguments]
  P -.-> H[Host validation and authorization remain external]
~~~

## Contract

The codec reads and writes the line-oriented CommandAi/1 envelope. Encoding uses LF line separators on every platform; decoding accepts LF and CRLF input. A command line contains a positive action identifier followed by ordered parameter/value pairs. Values are positive integer identifiers or JSON-quoted literal strings.

## Use cases

- Decode an untrusted envelope into bounded immutable data for host validation.
- Encode a constructed proposal for transport or inspection.
- Reject malformed envelopes, unsupported versions, duplicate declarations/parameters, invalid identifiers, and resource-limit violations.

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs). Parsing does not authorize or execute.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

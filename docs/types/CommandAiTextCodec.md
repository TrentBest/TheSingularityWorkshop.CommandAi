<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# CommandAiTextCodec

Encoder and decoder for the line-oriented CommandAi/1 envelope.

## Contract

- `Encode(proposal)` serializes the protocol header and ordered command lines.
- `Decode(text)` validates the envelope and reconstructs typed immutable data.
- Encoding emits LF line endings; decoding accepts LF or CRLF.
- Decoding removes empty lines and trims remaining lines. Whitespace-only lines are ignored as entries but still count toward the maximum-line guard; the opening and closing markers must be the first and last non-empty entries.
- Integer values are positive decimal identifiers.
- Literal values are JSON-quoted strings.
- Malformed input, unsupported versions, duplicate protocol declarations, duplicate parameters, invalid identifiers, and resource-limit violations are rejected.

Empty caller input is an argument error. Malformed non-empty wire input is a format error. Parsing does not authorize or execute.

Source: [CommandAiTextCodec.cs](../../CommandAiTextCodec.cs). Tests: [CommandAiTextCodecTests.cs](../../tests/CommandAi.Tests/CommandAiTextCodecTests.cs).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

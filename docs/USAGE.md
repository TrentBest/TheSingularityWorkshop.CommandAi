<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Use CommandAi

## Create a proposal

    using TheSingularityWorkshop.CommandAi;

    var proposal = new CommandProposal([
        new CommandInstruction(100, [
            new CommandArgument(200, CommandValue.FromIdentifier(302)),
            new CommandArgument(201, CommandValue.FromText("Logs.Diagnostic Stream"))
        ])
    ]);

The numbers are examples, not built-in meanings. Your host's trusted vocabulary must define what each identity means.

## Encode and decode

    var wireText = CommandAiTextCodec.Encode(proposal);
    var parsed = CommandAiTextCodec.Decode(wireText);

The codec validates representation only. Treat decoded proposals as untrusted input and perform host-specific resolution, validation, authorization, approval, and execution separately.

Decode distinguishes caller misuse from malformed wire content: null/empty/whitespace input throws ArgumentException; a non-empty but invalid envelope throws FormatException. This includes empty proposals, duplicate parameter identities, malformed literals, unsupported versions, and unknown entries. Direct model construction uses ArgumentException for invalid caller-supplied values.

## Use the generic builder or add host-owned names

    var proposal = new CommandProposalBuilder()
        .AddCommand(100, command => command
            .WithIdentifier(200, 302)
            .WithText(201, "Logs.Diagnostic Stream"))
        .Build();

A consuming host can layer typed extension methods such as InspectResource(...) above this generic builder. The names, ID mappings, and domain validation remain in the host; CommandAi does not register or execute them. See Builder integration and the compiled quickstart.

## Run the quickstart

    dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj

The example prints an encoded proposal, decodes it, and explicitly does not execute it.

## Verify the repository

    dotnet restore TheSingularityWorkshop.CommandAi.slnx
    dotnet build TheSingularityWorkshop.CommandAi.slnx --configuration Release
    dotnet test TheSingularityWorkshop.CommandAi.slnx --configuration Release
    dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj

The last previously recorded full verification is GitHub Actions run [#92](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/actions/runs/41283963463): build/test, quickstart execution, package packing, packed-artifact audit, and compilation against the packed package passed; NuGet publication was skipped. The decoder hardening and regression tests described above are newer changes and remain pending a fresh workflow result.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>

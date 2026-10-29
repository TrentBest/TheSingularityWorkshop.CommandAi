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

## Run the quickstart

    dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj

The example prints an encoded proposal, decodes it, and explicitly does not execute it.

## Verify the repository

    dotnet restore TheSingularityWorkshop.CommandAi.slnx
    dotnet build TheSingularityWorkshop.CommandAi.slnx --configuration Release
    dotnet test TheSingularityWorkshop.CommandAi.slnx --configuration Release
    dotnet run --project examples/CommandAi.QuickStart/CommandAi.QuickStart.csproj

The repository CI verified restore, Release build, all 9 unit tests, quickstart execution, and package packing at commit [`63dad94`](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/commit/63dad94c2d2feaf6344fdd6e3f180bfbbf8446de). The quickstart does not call a live provider and does not prove authorization or execution safety.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

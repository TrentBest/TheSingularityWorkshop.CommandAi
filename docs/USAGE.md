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

The quickstart is source-authored and awaits CI confirmation. Do not treat these instructions as evidence that a run has succeeded.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

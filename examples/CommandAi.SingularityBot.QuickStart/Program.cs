using TheSingularityWorkshop.CommandAi;
using TheSingularityWorkshop.CommandAi.Samples.SingularityBot;

// This is representative model output. In a real host, obtain it asynchronously
// from the configured LLM, then hand it to the same bounded decode/validation path.
const string modelOutput = """
[CommandAi]
Protocol=CommandAi/1
Command=1001 2001=1 2002="4"
[/CommandAi]
""";

try
{
    var proposal = CommandAiTextCodec.Decode(modelOutput);

    if (!BotCommandProposalReader.TryReadMarineProductionTarget(
            proposal, out var desiredCount, out var error))
    {
        Console.WriteLine($"Proposal rejected: {error}");
        return;
    }

    Console.WriteLine($"Validated proposal: SetProductionTarget(Marine, {desiredCount}).");
    Console.WriteLine("This sample stops at proposal validation.");
    Console.WriteLine("The live bot must still check its active BotDefinition, match state,");
    Console.WriteLine("host authorization, idempotency, and existing production runtime.");
    Console.WriteLine("No game state was changed and no SC2 action was dispatched.");
}
catch (FormatException exception)
{
    Console.WriteLine($"Malformed CommandAi response: {exception.Message}");
}

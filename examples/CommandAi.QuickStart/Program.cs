using TheSingularityWorkshop.CommandAi;
var proposal = new CommandProposal([new CommandInstruction(100, [
    new CommandArgument(200, CommandValue.FromIdentifier(302)),
    new CommandArgument(201, CommandValue.FromText("diagnostics"))
])]);
var encoded = CommandAiTextCodec.Encode(proposal);
Console.WriteLine(encoded);
Console.WriteLine();
Console.WriteLine($"Decoded {CommandAiTextCodec.Decode(encoded).Commands.Count} proposed command(s).");
Console.WriteLine("No command was executed. A host must resolve, validate, authorize, and execute any accepted proposal.");
using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .InspectResource(resourceId: 42)
    .OpenDiagnosticStream(streamName: "Diagnostics")
    .Build();

var encoded = CommandAiTextCodec.Encode(proposal);
Console.WriteLine(encoded);
Console.WriteLine();
Console.WriteLine($"Decoded {CommandAiTextCodec.Decode(encoded).Commands.Count} proposed command(s).");
Console.WriteLine("The named operations are host-owned examples. No command was executed.");
Console.WriteLine("A host must resolve identities, evaluate preconditions, validate current state, authorize, approve, and execute any accepted proposal.");

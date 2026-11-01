using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(100, command => command
        .WithIdentifier(200, 302)
        .WithText(201, "Logs.Diagnostic Stream"))
    .Build();

var encoded = CommandAiTextCodec.Encode(proposal);
Console.WriteLine(encoded);
Console.WriteLine();
Console.WriteLine($"Decoded {CommandAiTextCodec.Decode(encoded).Commands.Count} proposed command(s).");
Console.WriteLine("No command was executed. A host must resolve, validate, authorize, and execute any accepted proposal.");

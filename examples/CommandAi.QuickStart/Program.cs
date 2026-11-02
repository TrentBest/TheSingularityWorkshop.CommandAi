using TheSingularityWorkshop.CommandAi;

var proposal = new CommandProposalBuilder()
    .AddCommand(1001, command => command
        .WithIdentifier(parameterId: 2001, valueId: 42)
        .WithText(parameterId: 2002, value: "resource"))
    .AddCommand(1002, command => command
        .WithText(parameterId: 2003, value: "Diagnostics"))
    .Build();

var encoded = CommandAiTextCodec.Encode(proposal);
Console.WriteLine(encoded);
Console.WriteLine();
Console.WriteLine($"Decoded {CommandAiTextCodec.Decode(encoded).Commands.Count} proposed command(s).");
Console.WriteLine("The action and parameter identities are application-owned. No command was executed.");

using System.Collections.Concurrent;
using CompanyReceivingCapabilities;
using CompanyReceivingCapabilities.Models;
using CompanyReceivingCapabilities.Networks;
using Microsoft.Extensions.DependencyInjection;

//register resolvers
var sc = new ServiceCollection();
sc.AddCompanyReceivingCapabilityResolver();
sc.AddHttpClient();
sc.AddLogging();
var sp = sc.BuildServiceProvider();
var nea = sp.GetRequiredService<NeaResolver>();

//add a list of CustomerToMatch, set path to customer csv file
var csvCustomerRegistry = File.ReadAllLines(@"../../../customerlist_sample.csv");
var customers = new List<CustomerToMatch>();

foreach (var customer in csvCustomerRegistry)
{
    var lineSplit = customer.Split(';');
    customers.Add(new CustomerToMatch()
    {
        CustomerNumber = lineSplit[0].Trim(),
        Identifier = lineSplit[1].Trim(),
        CustomerName = lineSplit[2]
    });
}

// processes and stores file in specified dir with structure
// CustomerNumber;Identifier;customerName;Operator;DocumentType;FoundIdentifier");
var matches = new ConcurrentBag<CustomerCapability>();
var counter = 0;
await Parallel.ForEachAsync(
    customers,
    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
    async (customer, _) =>
    {
        var result = await nea.Match(customer, "SE");
        foreach (var res in result)
        {
            matches.Add(res);
        }
        Interlocked.Increment(ref counter);
        Console.Write($"\rProcessed: {counter} of {customers.Count} customers.");
    });

//set output path for result
var outputFilePath = @"../../../";
foreach (var m in matches)
{
    File.AppendAllText($"{outputFilePath}customer_list_{m.Network}.csv", 
        $"{m.CustomerNumber};{m.Identifier};{m.Name};{m.Operator};{m.DocumentType};{m.MatchedIdentifier}"
        + Environment.NewLine);
}
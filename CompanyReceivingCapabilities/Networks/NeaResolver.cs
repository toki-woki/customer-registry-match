using System.Net.Http.Json;
using CompanyReceivingCapabilities.Models;
using CompanyReceivingCapabilities.Models.Nea;
using Microsoft.Extensions.Logging;

namespace CompanyReceivingCapabilities.Networks;

public class NeaResolver
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<NeaResolver> _logger;

    public NeaResolver(
        IHttpClientFactory httpClientFactory, 
        ILogger<NeaResolver> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<CustomerCapability>> Match(
        CustomerToMatch customer, 
        string countryCode)
    {
        
        var neaIdentifier = await FindNeaInternalId(
            identifier: customer.Identifier, 
            countryCode:countryCode);
        
        var matches = new List<CustomerCapability>();
        if (neaIdentifier is not null)
        {
            var result = await ResolveCapabilities(
                customer: customer, 
                neaId: (int)neaIdentifier, 
                countryCode: countryCode);
            
            matches.AddRange(result);
        }
        
        return matches;
    }

    private async Task<int?> FindNeaInternalId(string countryCode, string identifier)
    {
        try
        {
            var reqAddress = $"https://eregister.nea.nu/server/Public/organizations?countryCode={countryCode}&searchText={identifier}";
            using var httpClient = _httpClientFactory.CreateClient();
            var resp = await httpClient.GetAsync(reqAddress);
            var foundOrg = await resp.Content.ReadFromJsonAsync<NeaInternalIdResult[]>();

            return foundOrg?.First().id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resolving Nea for {Identifier}.", identifier);
            return null;
        }
    }

    private async Task<List<CustomerCapability>> ResolveCapabilities(CustomerToMatch customer, int neaId, string countryCode)
    {
        var capabilities = new List<CustomerCapability>();
        try
        {
            _logger.LogDebug("Searching for {CustomerName} with {Identifier}.", customer.CustomerName, customer.Identifier);
            var reqAddress = $"https://eregister.nea.nu/server/Public/organizations/{neaId}?id={neaId}&countryCode={countryCode}";
            using var httpClient = _httpClientFactory.CreateClient();
            var resp = await httpClient.GetFromJsonAsync<NeaCompanyCapability>(reqAddress);

            var receivingCapabilities = resp?
                .organizationEaddress?
                .Where(x => x.directionOfAddress?.ToUpper() == "RECEIVE").ToList();

            if (receivingCapabilities != null && receivingCapabilities.Any())
            {
                foreach (var rec in receivingCapabilities)
                {
                    _logger.LogDebug("Found {CustomerName} with {Identifier} in {serviceId}.", 
                        customer.CustomerName, 
                        customer.Identifier,
                        rec.serviceId);
                    capabilities.Add(new CustomerCapability()
                    {
                        CustomerNumber = customer.CustomerNumber,
                        Identifier = customer.Identifier,
                        Name = customer.CustomerName,
                        Operator = rec.serviceId,
                        MatchedIdentifier = rec.identifier?.First(),
                        DocumentType = rec.contextOfAddress,
                        Network = Network.Nea
                    });
                }
            }
            _logger.LogDebug("No match found for {CustomerName} with {Identifier}", 
                customer.CustomerName, 
                customer.Identifier);
            
            return capabilities;
        }
        catch (Exception)
        {
            _logger.LogError("Error resolving {CustomerName} with {Identifier}",
                customer.CustomerName,
                customer.Identifier);
        }
        
        return Enumerable.Empty<CustomerCapability>().ToList();
    }
}
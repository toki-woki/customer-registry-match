namespace CompanyReceivingCapabilities.Models;

public class CustomerCapability
{
    public string? Operator { get; init; }
    public string? CustomerNumber { get; init; }
    public string? Identifier { get; init; }
    public string? Name { get; init; }
    public string? MatchedIdentifier { get; init; }
    public string? DocumentType { get; init; }
    public Network Network { get; init; }
}
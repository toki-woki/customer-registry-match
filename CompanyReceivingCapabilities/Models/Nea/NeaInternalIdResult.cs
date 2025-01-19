// ReSharper disable InconsistentNaming
namespace CompanyReceivingCapabilities.Models.Nea;

public class NeaInternalIdResult
{
    public string[]? identifier { get; set; }
    public string? name { get; set; }
    public object[]? street { get; set; }
    public int? id { get; set; }
    public Eaddress[]? eaddress { get; set; }
    public object[]? contact { get; set; }
    public object[]? peppolServiceGroup { get; set; }
}

public class Eaddress
{
    public string[]? identifier { get; set; }
    public string? name { get; set; }
}
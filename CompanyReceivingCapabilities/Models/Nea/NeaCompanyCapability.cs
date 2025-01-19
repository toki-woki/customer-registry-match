// ReSharper disable InconsistentNaming
namespace CompanyReceivingCapabilities.Models.Nea;

public class NeaCompanyCapability
{
    public Organization? organization { get; set; }
    public object[]? organizationContacts { get; set; }
    public OrganizationEaddress[]? organizationEaddress { get; set; }
    public object[]? peppolServiceGroup { get; set; }
    public object[]? peppolBusinesscard { get; set; }
}

public class Organization
{
    public string[]? identifier { get; set; }
    public string? name { get; set; }
    public object[]? additionalName { get; set; }
    public string[]? street { get; set; }
    public string? postalCode { get; set; }
    public string? city { get; set; }
    public string? country { get; set; }
    public string? countryCode { get; set; }
    public string? www { get; set; }
    public int? id { get; set; }
    public string? createTime { get; set; }
}

public class OrganizationEaddress
{
    public string[]? identifier { get; set; }
    public string? contextOfAddress { get; set; }
    public string? directionOfAddress { get; set; }
    public string? name { get; set; }
    public bool? permissionToSend { get; set; }
    public bool? ownerActive { get; set; }
    public bool? permissionToPublish { get; set; }
    public bool? supportAttachments { get; set; }
    public bool? specialRequirements { get; set; }
    public bool? primaryAddress { get; set; }
    public int? id { get; set; }
    public int? organizationId { get; set; }
    public int? ownerId { get; set; }
    public string? createTime { get; set; }
    public string? serviceId { get; set; }
    public string? serviceIdType { get; set; }
}
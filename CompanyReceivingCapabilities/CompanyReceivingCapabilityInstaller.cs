using CompanyReceivingCapabilities.Networks;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyReceivingCapabilities;

public static class CompanyReceivingCapabilityInstaller
{
    public static void AddCompanyReceivingCapabilityResolver(this IServiceCollection sc)
    {
        sc.AddTransient<NeaResolver>();
    }
}
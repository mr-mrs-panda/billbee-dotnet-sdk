using Microsoft.Extensions.Configuration;
using Panda.NuGet.BillbeeClient.Configs;

namespace Panda.NuGet.BillbeeClient.IntegrationTests;

internal sealed class TestConfiguration
{
    public required BillbeeApiConfig BillbeeApi { get; init; }

    public required IntegrationTestSettings IntegrationTests { get; init; }

    public static TestConfiguration Load()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build();

        var billbeeApi = configuration.GetSection("BillbeeApi").Get<BillbeeApiConfig>()
            ?? throw new InvalidOperationException("Missing BillbeeApi configuration section.");

        var integrationTests = configuration.GetSection("IntegrationTests").Get<IntegrationTestSettings>()
            ?? throw new InvalidOperationException("Missing IntegrationTests configuration section.");

        return new TestConfiguration
        {
            BillbeeApi = billbeeApi,
            IntegrationTests = integrationTests
        };
    }

    public bool UsesPlaceholders()
    {
        return IsPlaceholder(BillbeeApi.Username, "YOUR_BILLBEE_USERNAME")
            || IsPlaceholder(BillbeeApi.Password, "YOUR_BILLBEE_API_PASSWORD")
            || IsPlaceholder(BillbeeApi.ApiKey, "YOUR_BILLBEE_API_KEY");
    }

    private static bool IsPlaceholder(string? value, string placeholder)
    {
        return string.IsNullOrWhiteSpace(value) || string.Equals(value, placeholder, StringComparison.Ordinal);
    }
}

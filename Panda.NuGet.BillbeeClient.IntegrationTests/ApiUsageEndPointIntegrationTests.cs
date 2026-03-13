using Microsoft.Extensions.DependencyInjection;
using Panda.NuGet.BillbeeClient.Extensions;
using Panda.NuGet.BillbeeClient.Models;
using Xunit;

namespace Panda.NuGet.BillbeeClient.IntegrationTests;

public class ApiUsageEndPointIntegrationTests
{
    private readonly TestConfiguration _configuration = TestConfiguration.Load();

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetSummarisedApiUsageAsync_Returns_Result()
    {
        EnsureConfigured();

        await using var scope = CreateServiceProvider().CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<IBillbeeClient>();

        var result = await client.ApiUsage.GetSummarisedApiUsageAsync(new ApiUsageRequest
        {
            Date = _configuration.IntegrationTests.ApiUsageDate
        });

        Assert.NotNull(result);
        Assert.True(result.Success, result.ErrorMessage ?? "The API request returned an unsuccessful result.");
        Assert.NotNull(result.Data);
    }

    [SkippableFact]
    [Trait("Category", "Integration")]
    public async Task GetDetailedApiUsageAsync_Returns_Result()
    {
        EnsureConfigured();

        await using var scope = CreateServiceProvider().CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<IBillbeeClient>();

        var result = await client.ApiUsage.GetDetailedApiUsageAsync(new ApiUsageRequest
        {
            Date = _configuration.IntegrationTests.ApiUsageDate
        });

        Assert.NotNull(result);
        Assert.True(result.Success, result.ErrorMessage ?? "The API request returned an unsuccessful result.");
        Assert.NotNull(result.Data);
    }

    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddBillbeeApi(_configuration.BillbeeApi);
        return services.BuildServiceProvider();
    }

    private void EnsureConfigured()
    {
        Skip.If(
            _configuration.UsesPlaceholders(),
            "Integration test configuration uses placeholders. Copy appsettings.local.json.example to appsettings.local.json and fill in your Billbee credentials.");
    }
}

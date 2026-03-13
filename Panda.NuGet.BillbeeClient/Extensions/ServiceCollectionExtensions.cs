using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Panda.NuGet.BillbeeClient.Configs;
using Panda.NuGet.BillbeeClient.Endpoints;
using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;
using Panda.NuGet.BillbeeClient.Utilities;

namespace Panda.NuGet.BillbeeClient.Extensions;

public static class ServiceCollectionExtensions
{
    private const string ClientName = nameof(BillbeeClient);

    public static void AddBillbeeApi(
        this IServiceCollection serviceCollection, 
        BillbeeApiConfig config,
        Action<HttpClient>? configureClient = null)
    {
        ValidateConfig(config);

        serviceCollection.AddSingleton(config);
        serviceCollection.AddScoped<IRateLimiter, RateLimiter>();
        
        serviceCollection.AddHttpClient(ClientName, client =>
        {
            ConfigureHttpClient(client, config);
            configureClient?.Invoke(client);
        });

        AddScopedClients(serviceCollection);
        
        serviceCollection.AddScoped<IBillbeeClient, BillbeeClient>();
    }

    private static void ValidateConfig(BillbeeApiConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.Username))
        {
            throw new ArgumentException("Username is required", nameof(config.Username));
        }

        if (string.IsNullOrWhiteSpace(config.Password))
        {
            throw new ArgumentException("Password is required", nameof(config.Password));
        }

        if (string.IsNullOrWhiteSpace(config.ApiKey))
        {
            throw new ArgumentException("ApiKey is required", nameof(config.ApiKey));
        }

        if (string.IsNullOrWhiteSpace(config.BaseUrl))
        {
            throw new ArgumentException("ApiKey is required", nameof(config.BaseUrl));
        }
    }

    private static void AddScopedClients(IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IBillbeeRestClient, BillbeeRestClient>(ClientName);
        serviceCollection.AddScoped<IApiUsageEndPoint, ApiUsageEndPoint>();
        serviceCollection.AddScoped<IAutomaticProvisionEndPoint, AutomaticProvisionEndPoint>();
        serviceCollection.AddScoped<ICloudStoragesEndPoint, CloudStoragesEndPoint>();
        serviceCollection.AddScoped<ICustomerAddressesEndPoint, CustomerAddressesEndPoint>();
        serviceCollection.AddScoped<ICustomerEndPoint, CustomerEndPoint>();
        serviceCollection.AddScoped<IEnumEndPoint, EnumEndPoint>();
        serviceCollection.AddScoped<IEventEndPoint, EventEndPoint>();
        serviceCollection.AddScoped<IOrderEndPoint, OrderEndPoint>();
        serviceCollection.AddScoped<IProductEndPoint, ProductEndPoint>();
        serviceCollection.AddScoped<ISearchEndPoint, SearchEndPoint>();
        serviceCollection.AddScoped<IShipmentEndPoint, ShipmentEndPoint>();
        serviceCollection.AddScoped<IWebhookEndPoint, WebhookEndPoint>();
    }
    
    private static void ConfigureHttpClient(HttpClient httpClient, BillbeeApiConfig config)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{config.Username}:{config.Password}")));
        httpClient.DefaultRequestHeaders.Add("X-Billbee-Api-Key", config.ApiKey);
    }
}

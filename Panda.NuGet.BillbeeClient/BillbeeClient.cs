using Microsoft.Extensions.DependencyInjection;
using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;

namespace Panda.NuGet.BillbeeClient
{
    /// <inheritdoc cref="IBillbeeClient"/>
    public class BillbeeClient : IBillbeeClient
    {
        private readonly IServiceProvider _serviceProvider;

        public BillbeeClient(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IApiUsageEndPoint ApiUsage => _serviceProvider.GetRequiredService<IApiUsageEndPoint>();
        public IEventEndPoint Events => _serviceProvider.GetRequiredService<IEventEndPoint>();
        public IShipmentEndPoint Shipment => _serviceProvider.GetRequiredService<IShipmentEndPoint>();
        public IWebhookEndPoint Webhooks => _serviceProvider.GetRequiredService<IWebhookEndPoint>();
        public IProductEndPoint Products => _serviceProvider.GetRequiredService<IProductEndPoint>();

        public IAutomaticProvisionEndPoint AutomaticProvision =>
            _serviceProvider.GetRequiredService<IAutomaticProvisionEndPoint>();

        public ICustomerEndPoint Customer => _serviceProvider.GetRequiredService<ICustomerEndPoint>();

        public ICustomerAddressesEndPoint CustomerAddresses =>
            _serviceProvider.GetRequiredService<ICustomerAddressesEndPoint>();

        public ISearchEndPoint Search => _serviceProvider.GetRequiredService<ISearchEndPoint>();
        public IOrderEndPoint Orders => _serviceProvider.GetRequiredService<IOrderEndPoint>();
        public ICloudStoragesEndPoint CloudStorages => _serviceProvider.GetRequiredService<ICloudStoragesEndPoint>();
        public IEnumEndPoint Enums => _serviceProvider.GetRequiredService<IEnumEndPoint>();
    }
}

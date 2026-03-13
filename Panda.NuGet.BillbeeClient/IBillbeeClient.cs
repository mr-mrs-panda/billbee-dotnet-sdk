using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;

namespace Panda.NuGet.BillbeeClient
{
    /// <summary>
    /// Client for the Billbee API
    /// see https://app.billbee.io/swagger/ui/index or https://www.billbee.de/api/ for further information
    /// </summary>
    public interface IBillbeeClient
    {
        /// <summary>
        /// EndPoint to access API usage statistics
        /// </summary>
        IApiUsageEndPoint ApiUsage { get; }

        /// <summary>
        /// EndPoint to access events
        /// </summary>
        IEventEndPoint Events { get; }

        /// <summary>
        /// EndPoint to access order independent shipments
        /// </summary>
        IShipmentEndPoint Shipment { get; }

        /// <summary>
        /// EndPoint to access webhooks
        /// </summary>
        IWebhookEndPoint Webhooks { get; }

        /// <summary>
        /// EndPoint to access Products
        /// </summary>
        IProductEndPoint Products { get; }

        /// <summary>
        /// EndPoint to allow automatic user creation
        /// </summary>
        IAutomaticProvisionEndPoint AutomaticProvision { get; }

        /// <summary>
        /// EndPoint to access customer base data
        /// </summary>
        ICustomerEndPoint Customer { get; }

        /// <summary>
        /// EndPoint to access customer addresses
        /// </summary>
        ICustomerAddressesEndPoint CustomerAddresses { get; }

        /// <summary>
        /// EndPoint for searches in customers, orders and products
        /// </summary>
        ISearchEndPoint Search { get; }

        /// <summary>
        /// EndPoint to access orders
        /// </summary>
        IOrderEndPoint Orders { get; }

        /// <summary>
        /// EndPoint to access cloud storages
        /// </summary>
        ICloudStoragesEndPoint CloudStorages { get; }
        
        /// <summary>
        /// EndPoint to access enum values
        /// </summary>
        IEnumEndPoint Enums { get; }
    }
}

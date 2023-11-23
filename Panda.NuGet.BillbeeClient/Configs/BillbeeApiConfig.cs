using Panda.NuGet.BillbeeClient.Enums;

namespace Panda.NuGet.BillbeeClient.Configs
{
    /// <summary>
    /// Configuration parameters for the Billbee API client.
    /// </summary>
    public class BillbeeApiConfig
    {
        /// <summary>
        /// Username of the main user of your account.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// API password, could be set in Settings->Billbee API->General Settings
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// The Api key for your application. Can be requested from the Billbee support.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// The base url of the Billbee API. Typically, this sticks unchanged.
        /// </summary>
        public string? BaseUrl { get; set; } = "https://app.billbee.io/api/v1";

        /// <summary>
        /// The timout in seconds for the API calls. If not set, the default timeout of the HttpClient is used.
        /// </summary>
        public int? Timeout { get; set; }
    }
}

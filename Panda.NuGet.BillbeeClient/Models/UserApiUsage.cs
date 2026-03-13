namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Detailed API usage for a single user.
    /// </summary>
    public class UserApiUsage
    {
        /// <summary>
        /// The Billbee user name.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Per-endpoint usage metrics for the user.
        /// </summary>
        public List<EndpointUsageMetric>? Data { get; set; }
    }
}

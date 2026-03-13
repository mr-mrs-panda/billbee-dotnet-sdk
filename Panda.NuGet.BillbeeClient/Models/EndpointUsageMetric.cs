namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Usage metrics for a single HTTP method and endpoint combination.
    /// </summary>
    public class EndpointUsageMetric
    {
        /// <summary>
        /// The HTTP method.
        /// </summary>
        public string? Method { get; set; }

        /// <summary>
        /// The Billbee API endpoint path.
        /// </summary>
        public string? Endpoint { get; set; }

        /// <summary>
        /// The number of recorded API calls.
        /// </summary>
        public long Count { get; set; }
    }
}

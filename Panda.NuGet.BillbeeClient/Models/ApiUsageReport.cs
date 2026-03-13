namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Detailed API usage grouped by user and endpoint.
    /// </summary>
    public class ApiUsageReport
    {
        /// <summary>
        /// The detailed usage entries.
        /// </summary>
        public List<UserApiUsage>? Details { get; set; }
    }
}

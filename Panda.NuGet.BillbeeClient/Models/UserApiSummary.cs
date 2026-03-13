namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Summarised API usage for a single user.
    /// </summary>
    public class UserApiSummary
    {
        /// <summary>
        /// The Billbee user name.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// The aggregated usage count.
        /// </summary>
        public UsageCount? Data { get; set; }
    }
}

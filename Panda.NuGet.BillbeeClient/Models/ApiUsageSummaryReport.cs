namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Summarised API usage grouped by user.
    /// </summary>
    public class ApiUsageSummaryReport
    {
        /// <summary>
        /// The summarised usage entries.
        /// </summary>
        public List<UserApiSummary>? Summaries { get; set; }
    }
}

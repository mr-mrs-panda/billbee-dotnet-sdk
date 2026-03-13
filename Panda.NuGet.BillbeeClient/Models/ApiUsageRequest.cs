namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Filter request for API usage reports.
    /// </summary>
    public class ApiUsageRequest
    {
        /// <summary>
        /// Only year and month of this value are relevant.
        /// </summary>
        public DateTime? Date { get; set; }
    }
}

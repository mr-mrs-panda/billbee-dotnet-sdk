namespace Panda.NuGet.BillbeeClient.Models
{
    /// <summary>
    /// Holds a single usage counter value.
    /// </summary>
    public class UsageCount
    {
        /// <summary>
        /// The number of recorded API calls.
        /// </summary>
        public long Count { get; set; }
    }
}

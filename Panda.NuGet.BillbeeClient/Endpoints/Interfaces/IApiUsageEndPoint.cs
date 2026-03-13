using Panda.NuGet.BillbeeClient.Models;

namespace Panda.NuGet.BillbeeClient.Endpoints.Interfaces
{
    /// <summary>
    /// EndPoint to access API usage statistics.
    /// </summary>
    public interface IApiUsageEndPoint
    {
        /// <summary>
        /// Gets summarised API usage statistics.
        /// </summary>
        /// <param name="request">The optional year/month filter.</param>
        /// <returns>Summarised usage grouped by user.</returns>
        Task<ApiResult<ApiUsageSummaryReport>> GetSummarisedApiUsageAsync(ApiUsageRequest? request = null);

        /// <summary>
        /// Gets detailed API usage statistics.
        /// </summary>
        /// <param name="request">The optional year/month filter.</param>
        /// <returns>Detailed usage grouped by user and endpoint.</returns>
        Task<ApiResult<ApiUsageReport>> GetDetailedApiUsageAsync(ApiUsageRequest? request = null);
    }
}

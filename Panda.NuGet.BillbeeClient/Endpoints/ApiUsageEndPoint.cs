using System.Collections.Specialized;
using System.Globalization;
using System.Text.Json;
using Panda.NuGet.BillbeeClient.Endpoints.Interfaces;
using Panda.NuGet.BillbeeClient.Models;

namespace Panda.NuGet.BillbeeClient.Endpoints
{
    /// <inheritdoc cref="IApiUsageEndPoint" />
    public class ApiUsageEndPoint : IApiUsageEndPoint
    {
        private readonly IBillbeeRestClient _restClient;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        /// <summary>
        /// Creates a new endpoint wrapper for API usage statistics.
        /// </summary>
        /// <param name="restClient">The underlying REST client.</param>
        public ApiUsageEndPoint(IBillbeeRestClient restClient)
        {
            _restClient = restClient;
        }

        /// <inheritdoc />
        public async Task<ApiResult<ApiUsageSummaryReport>> GetSummarisedApiUsageAsync(ApiUsageRequest? request = null)
        {
            return await GetApiUsageAsync<ApiUsageSummaryReport>("/api/v1/apiusage", request);
        }

        /// <inheritdoc />
        public async Task<ApiResult<ApiUsageReport>> GetDetailedApiUsageAsync(ApiUsageRequest? request = null)
        {
            return await GetApiUsageAsync<ApiUsageReport>("/api/v1/apiusage/detail", request);
        }

        private static NameValueCollection? BuildParameters(ApiUsageRequest? request)
        {
            if (request?.Date == null)
            {
                return null;
            }

            return new NameValueCollection
            {
                { "request.date", request.Date.Value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) }
            };
        }

        private async Task<ApiResult<T>> GetApiUsageAsync<T>(string resource, ApiUsageRequest? request) where T : class
        {
            var content = await _restClient.GetStringAsync(resource, BuildParameters(request));
            using var jsonDocument = JsonDocument.Parse(content);

            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object &&
                jsonDocument.RootElement.TryGetProperty("Data", out _))
            {
                var wrappedResult = JsonSerializer.Deserialize<ApiResult<T>>(content, JsonOptions);
                if (wrappedResult != null)
                {
                    return wrappedResult;
                }
            }

            var rawResult = JsonSerializer.Deserialize<T>(content, JsonOptions)
                ?? throw new JsonException($"Could not deserialize response from '{resource}'.");

            return new ApiResult<T>
            {
                Data = rawResult
            };
        }
    }
}

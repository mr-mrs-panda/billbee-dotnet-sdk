using System.Collections.Specialized;
using Moq;
using Panda.NuGet.BillbeeClient.Endpoints;
using Panda.NuGet.BillbeeClient.Models;
using Xunit;

namespace Panda.NuGet.BillbeeClient.Tests;

public class ApiUsageEndPointTests
{
    [Fact]
    public async Task GetSummarisedApiUsageAsync_Maps_Raw_Response()
    {
        var restClientMock = CreateRestClientMock(RawSummaryResponse);
        var endpoint = new ApiUsageEndPoint(restClientMock.Object);

        var result = await endpoint.GetSummarisedApiUsageAsync(new ApiUsageRequest
        {
            Date = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        var summaries = AssertSuccessfulSummary(result);
        var summary = Assert.Single(summaries);
        Assert.Equal("raw@example.com", summary.UserName);
        Assert.Equal(42, summary.Data!.Count);
        VerifyGetStringCalled(restClientMock, "/api/v1/apiusage", "2026-03-01T00:00:00");
    }

    [Fact]
    public async Task GetSummarisedApiUsageAsync_Maps_Wrapped_Response()
    {
        var restClientMock = CreateRestClientMock(WrappedSummaryResponse);
        var endpoint = new ApiUsageEndPoint(restClientMock.Object);

        var result = await endpoint.GetSummarisedApiUsageAsync();

        var summaries = AssertSuccessfulSummary(result);
        var summary = Assert.Single(summaries);
        Assert.Equal("wrapped@example.com", summary.UserName);
        Assert.Equal(7, summary.Data!.Count);
        VerifyGetStringCalled(restClientMock, "/api/v1/apiusage");
    }

    [Fact]
    public async Task GetDetailedApiUsageAsync_Maps_Raw_Response()
    {
        var restClientMock = CreateRestClientMock(RawDetailResponse);
        var endpoint = new ApiUsageEndPoint(restClientMock.Object);

        var result = await endpoint.GetDetailedApiUsageAsync();

        var detailEntries = AssertSuccessfulDetails(result);
        var details = Assert.Single(detailEntries);
        var metric = Assert.Single(details.Data!);
        Assert.Equal("detail@example.com", details.UserName);
        Assert.Equal("GET", metric.Method);
        Assert.Equal("/orders", metric.Endpoint);
        Assert.Equal(12, metric.Count);
        VerifyGetStringCalled(restClientMock, "/api/v1/apiusage/detail");
    }

    [Fact]
    public async Task GetDetailedApiUsageAsync_Maps_Wrapped_Response()
    {
        var restClientMock = CreateRestClientMock(WrappedDetailResponse);
        var endpoint = new ApiUsageEndPoint(restClientMock.Object);

        var result = await endpoint.GetDetailedApiUsageAsync();

        var detailEntries = AssertSuccessfulDetails(result);
        var details = Assert.Single(detailEntries);
        var metric = Assert.Single(details.Data!);
        Assert.Equal("wrapped-detail@example.com", details.UserName);
        Assert.Equal("POST", metric.Method);
        Assert.Equal("/products", metric.Endpoint);
        Assert.Equal(3, metric.Count);
        VerifyGetStringCalled(restClientMock, "/api/v1/apiusage/detail");
    }

    [Fact]
    public async Task GetDetailedApiUsageAsync_Invalid_Json_Throws()
    {
        var restClientMock = CreateRestClientMock("not-json");
        var endpoint = new ApiUsageEndPoint(restClientMock.Object);

        await Assert.ThrowsAnyAsync<System.Text.Json.JsonException>(() => endpoint.GetDetailedApiUsageAsync());
        VerifyGetStringCalled(restClientMock, "/api/v1/apiusage/detail");
    }

    private static List<UserApiSummary> AssertSuccessfulSummary(ApiResult<ApiUsageSummaryReport> result)
    {
        Assert.True(result.Success);
        var data = Assert.IsType<ApiUsageSummaryReport>(result.Data);
        return Assert.IsType<List<UserApiSummary>>(data.Summaries);
    }

    private static List<UserApiUsage> AssertSuccessfulDetails(ApiResult<ApiUsageReport> result)
    {
        Assert.True(result.Success);
        var data = Assert.IsType<ApiUsageReport>(result.Data);
        return Assert.IsType<List<UserApiUsage>>(data.Details);
    }

    private static Mock<IBillbeeRestClient> CreateRestClientMock(string responseBody)
    {
        var restClientMock = new Mock<IBillbeeRestClient>(MockBehavior.Strict);
        restClientMock
            .Setup(client => client.GetStringAsync(It.IsAny<string>(), It.IsAny<NameValueCollection?>()))
            .ReturnsAsync(responseBody);

        return restClientMock;
    }

    private static void VerifyGetStringCalled(
        Mock<IBillbeeRestClient> restClientMock,
        string expectedResource,
        string? expectedRequestDate = null)
    {
        restClientMock.Verify(client => client.GetStringAsync(
            expectedResource,
            It.Is<NameValueCollection?>(parameters => HasExpectedRequestDate(parameters, expectedRequestDate))),
            Times.Once);
    }

    private static bool HasExpectedRequestDate(NameValueCollection? parameters, string? expectedRequestDate)
    {
        if (expectedRequestDate is null)
        {
            return parameters is null;
        }

        return parameters?["request.date"] == expectedRequestDate;
    }

    private const string RawSummaryResponse = """
        {
          "Summaries": [
            {
              "UserName": "raw@example.com",
              "Data": {
                "Count": 42
              }
            }
          ]
        }
        """;

    private const string WrappedSummaryResponse = """
        {
          "ErrorMessage": null,
          "ErrorCode": 0,
          "Data": {
            "Summaries": [
              {
                "UserName": "wrapped@example.com",
                "Data": {
                  "Count": 7
                }
              }
            ]
          }
        }
        """;

    private const string RawDetailResponse = """
        {
          "Details": [
            {
              "UserName": "detail@example.com",
              "Data": [
                {
                  "Method": "GET",
                  "Endpoint": "/orders",
                  "Count": 12
                }
              ]
            }
          ]
        }
        """;

    private const string WrappedDetailResponse = """
        {
          "ErrorMessage": null,
          "ErrorCode": 0,
          "Data": {
            "Details": [
              {
                "UserName": "wrapped-detail@example.com",
                "Data": [
                  {
                    "Method": "POST",
                    "Endpoint": "/products",
                    "Count": 3
                  }
                ]
              }
            ]
          }
        }
        """;
}

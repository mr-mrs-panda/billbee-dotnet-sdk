# Panda.NuGet.BillbeeClient

A modern .NET library for seamless integration with the Billbee API, using `System.Net.Http` and `System.Text.Json` for efficient, dependency-light communication.

## About

This project was originally forked from [billbeeio/billbee-csharp-sdk](https://github.com/billbeeio/billbee-csharp-sdk) but now maintains its own independent git history. The library has been completely rewritten to leverage modern .NET libraries and practices.

## Features

- **Modern .NET Support**: Targets .NET 8.0, 9.0 and 10.0
- **Performance Optimized**: Uses `System.Net.Http` and `System.Text.Json` for maximum efficiency
- **Dependency Light**: Minimal external dependencies for easier maintenance
- **Built-in Rate Limiting**: Prevents API rate limit violations
- **Comprehensive API Coverage**: Supports all major Billbee API endpoints

### Supported Endpoints

- Orders and order management
- Products and inventory
- Customers and addresses
- Shipments and tracking
- Webhooks and events
- Cloud storage integration
- Automatic provisioning
- Search functionality

## Installation

The package is available on [NuGet.org](https://www.nuget.org/packages/Panda.NuGet.BillbeeClient/):

```bash
dotnet add package Panda.NuGet.BillbeeClient
```

Or via Package Manager Console:

```powershell
Install-Package Panda.NuGet.BillbeeClient
```

## Quick Start

### 1. Configure Services

```csharp
using Panda.NuGet.BillbeeClient.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBillbeeApi(new BillbeeApiConfig
        {
            Username = "your-username",
            Password = "your-password", 
            ApiKey = "your-api-key",
            BaseUrl = "https://api.billbee.io"
        });
    }
}
```

### 2. Use in Your Services

```csharp
public class OrderService
{
    private readonly IBillbeeClient _billbeeClient;

    public OrderService(IBillbeeClient billbeeClient)
    {
        _billbeeClient = billbeeClient;
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync()
    {
        var result = await _billbeeClient.Orders.GetOrdersAsync();
        return result.Data;
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        var result = await _billbeeClient.Products.GetProductAsync(productId);
        return result.Data;
    }
}
```

## Configuration

The `BillbeeApiConfig` supports the following properties:

- `Username`: Your Billbee username
- `Password`: Your Billbee password or API password
- `ApiKey`: Your Billbee API key
- `BaseUrl`: The Billbee API base URL (default: https://api.billbee.io)

## Rate Limiting

The client includes built-in rate limiting to prevent API quota violations. The rate limiter is implemented as a singleton and will work correctly within a single application instance.

## Documentation

For detailed API documentation, refer to:
- [Official Billbee API Documentation](https://app.billbee.io/swagger/ui/index)
- [Billbee Website](https://www.billbee.de/api/)

## Requirements

- .NET 8.0, 9.0 or 10.0
- Valid Billbee API credentials

## Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

Originally forked from [billbeeio/billbee-csharp-sdk](https://github.com/billbeeio/billbee-csharp-sdk) - thanks to the original contributors for the foundation.
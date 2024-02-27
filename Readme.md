# Panda.NuGet.BillbeeClient

Panda.NuGet.BillbeeClient is a .NET Core library designed to provide an easy-to-use interface for communicating with the Billbee API. This project is a fork of the [billbeeio/billbee-csharp-sdk](https://github.com/billbeeio/billbee-csharp-sdk) repository.

## Goals

The primary goal of this library is to provide a modern, efficient, and streamlined way to interact with the Billbee API. Specifically, it employs `System.Net.Http` for making HTTP requests and `System.Text.Json` for JSON serialization and deserialization. These choices move away from the RestSharp and Newtonsoft.Json libraries used in the original repository.

### Advantages

1. **Consistency**: Utilizing native .NET Core libraries like `System.Net.Http` and `System.Text.Json` brings a level of consistency and integration that external libraries may not offer.

2. **Performance**: Both `System.Net.Http` and `System.Text.Json` are optimized for performance, which can be crucial for high-throughput applications.

3. **Ease of Maintenance**: Using built-in libraries means fewer external dependencies, making it easier to maintain and update the project.

4. **Forward Compatibility**: Leveraging core libraries ensures better forward compatibility with future .NET Core updates.

5. **Breaking Changes**: Moving away from RestSharp eliminates the risk posed by breaking changes in external libraries, providing a more stable foundation for the project.

By aligning with the .NET Core ecosystem, this library aims for robustness, efficiency, and ease of use.

## Rate Limiting

This project implements request rate limiting through a singleton RateLimiter. This ensures that API calls do not exceed the set rate limits. Note, however, that this implementation will only function reliably if multiple instances of the application are not running concurrently.

## Installation

The Panda.NuGet.BillbeeClient library is available as a NuGet package. You can install it via the NuGet package manager from the GitLab repository [pandapknaepel/billbee-csharp-sdk](https://gitlab.com/pknaepel_panda/billbee-csharp-sdk/).

### 1. Add a source with the .NET CLI

~~~bash
dotnet nuget add source "https://gitlab.com/api/v4/projects/52452560/packages/nuget/index.json" --name "GitLab Panda.NuGet.BillbeeClient"
~~~

### 2. Install a package with the .NET CLI

```bash
dotnet add package Panda.NuGet.BillbeeClient --source "GitLab Panda.NuGet.BillbeeClient" --version <version>
```

Replace `<version>` with the latest version number.

## Usage

To use the Panda.NuGet.BillbeeClient in your project, first, you need to configure the services in the `Startup.cs` file of your project.

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

You can now inject the `IBillbeeClient` interface wherever you need to use the Billbee API.

```csharp
public class MyService
{
    private readonly IBillbeeClient _billbeeClient;

    public MyService(IBillbeeClient billbeeClient)
    {
        _billbeeClient = billbeeClient;
    }

    public async Task DoSomethingAsync()
    {
        var events = await _billbeeClient.Events.GetEventsAsync();
        // ...
    }
}
```

## Features

Panda.NuGet.BillbeeClient supports various endpoints allowing access to different sections of the Billbee API, such as events, shipments, webhooks, products, automatic user creation, customer base data, customer addresses, orders, cloud storages, and enum values.

## Documentation

For more details on the Billbee API and what you can do with it, please refer to the [official Billbee API documentation](https://app.billbee.io/swagger/ui/index) or [Billbee's website](https://www.billbee.de/api/).

## Contributing

If you find any issues or have suggestions for improvements, feel free to open an issue or create a pull request. Your contributions are welcome!

## License

This project inherits the license from the original [billbeeio/billbee-csharp-sdk](https://github.com/billbeeio/billbee-csharp-sdk) repository.
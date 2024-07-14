using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace ProductCatalog.Api.Configuration;

public static class ConfigureLogging
{
    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
            builder.Logging.AddFilter("Microsoft.AspNetCore.Routing", LogLevel.Debug);
        }
    }
}

using HealthChecks.UI.Core;
using Infrastructure.Serialization.Abstraction;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAppsGateway.Services.Implementation;

public class HealthCheckResponseWriter
{
    private static IDataSerializer? _serializer = null;

    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        if (_serializer is null)
        {
            var services = context.RequestServices;
            _serializer = services.GetRequiredService<IDataSerializer>();
        }

        context.Response.ContentType = "application/json";

        var result = UIHealthReport.CreateFrom(report);
        var json = _serializer.Serialize(result);

        await context.Response.WriteAsync(json);
    }
}

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAppsGateway.Services.Implementation;

public class HealthCheckResponseWriter
{
    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            entries = report.Entries.Select(e => new
            {
                key = e.Key,
                value = new
                {
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration,
                    exception = e.Value.Exception?.Message,
                    data = e.Value.Data
                }
            })
        };

        var json = JsonSerializer.Serialize(result);

        await context.Response.WriteAsync(json);
    }
}

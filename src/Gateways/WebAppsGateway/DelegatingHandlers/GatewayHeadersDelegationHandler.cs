using Microsoft.Extensions.Options;

namespace WebAppsGateway.DelegatingHandlers;

public class GatewayHeadersDelegationHandler(IOptions<GatewaySettings> options) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        if (options.Value.UseAsIssuer)
        {
            request.Headers.Add("Use-Gateway-As-Identity", "true");
            request.Headers.Add("Gateway-Url", options.Value.Url);
        }

        return await base.SendAsync(request, token);
    }
}

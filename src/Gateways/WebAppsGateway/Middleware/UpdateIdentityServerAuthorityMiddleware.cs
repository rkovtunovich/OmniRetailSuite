﻿using Consul;
using IdentityServer4.AccessTokenValidation;
using Microsoft.Extensions.Options;

namespace WebAppsGateway.Middleware;

public class UpdateIdentityServerAuthorityMiddleware(RequestDelegate next, IConsulClient consulClient, ILogger<UpdateIdentityServerAuthorityMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly IConsulClient _consulClient = consulClient;

    private string? _cachedAuthority;
    private DateTime _lastUpdated;

    public async Task InvokeAsync(HttpContext context, IOptionsMonitor<IdentityServerAuthenticationOptions> optionsAccessor)
    {
        if(context.Request.Path.StartsWithSegments("/_health", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (_cachedAuthority is null || CacheIsStale())
        {
            var identityService = await _consulClient.Catalog.Service("identity");
            var serviceEntry = identityService.Response.FirstOrDefault();

            if (serviceEntry is not null)
            {
                _cachedAuthority = $"https://{serviceEntry.ServiceAddress}:{serviceEntry.ServicePort}";
                _lastUpdated = DateTime.UtcNow;
            }
        }

        if (_cachedAuthority is not null)
        {
            var options = optionsAccessor.Get("IdentityServer");
            options.Authority = _cachedAuthority;
        }

        logger.LogDebug($"Ocelot: remote port {context.Connection.RemotePort}");

        await _next(context);
    }

    private bool CacheIsStale() 
    {
        return (DateTime.UtcNow - _lastUpdated).TotalMinutes > 10;  // Refresh every 10 minutes
    }
}

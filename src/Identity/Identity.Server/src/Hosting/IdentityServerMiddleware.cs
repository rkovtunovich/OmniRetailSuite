﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.Hosting;

/// <summary>
/// IdentityServer middleware
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="IdentityServerMiddleware"/> class.
/// </remarks>
/// <param name="next">The next.</param>
/// <param name="logger">The logger.</param>
public class IdentityServerMiddleware(RequestDelegate next, ILogger<IdentityServerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="router">The router.</param>
    /// <param name="session">The user session.</param>
    /// <param name="events">The event service.</param>
    /// <param name="backChannelLogoutService"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context, IEndpointRouter router, IUserSession session, IEventService events, IBackChannelLogoutService backChannelLogoutService)
    {
        // this will check the authentication session and from it emit the check session
        // cookie needed from JS-based signout clients.
        await session.EnsureSessionIdCookieAsync();

        context.Response.OnStarting(async () =>
        {
            if (context.GetSignOutCalled())
            {
                _logger.LogDebug("SignOutCalled set; processing post-signout session cleanup.");

                // this clears our session id cookie so JS clients can detect the user has signed out
                await session.RemoveSessionIdCookieAsync();

                // back channel logout
                var logoutContext = await session.GetLogoutNotificationContext();
                if (logoutContext is not null)               
                    await backChannelLogoutService.SendLogoutNotificationsAsync(logoutContext);              
            }
        });

        try
        {
            var endpoint = router.Find(context);
            if (endpoint is not null)
            {
                _logger.LogInformation($"Invoking IS4 endpoint: {endpoint.GetType().FullName} for {context.Request.Path}. Remote host: {context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}");

                var result = await endpoint.ProcessAsync(context);

                if (result is not null)
                {
                    _logger.LogTrace("Invoking result: {type}", result.GetType().FullName);
                    await result.ExecuteAsync(context);
                }

                return;
            }
        }
        catch (Exception ex)
        {
            await events.RaiseAsync(new UnhandledExceptionEvent(ex));
            _logger.LogCritical(ex, "Unhandled exception: {exception}", ex.Message);
            throw;
        }

        await _next(context);
    }
}

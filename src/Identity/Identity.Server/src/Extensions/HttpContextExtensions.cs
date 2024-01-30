﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer4.Extensions;

public static class HttpContextExtensions
{
    public static async Task<bool> GetSchemeSupportsSignOutAsync(this HttpContext context, string scheme)
    {
        var provider = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await provider.GetHandlerAsync(context, scheme);
        return (handler is IAuthenticationSignOutHandler);
    }

    private static readonly string[] _separator = ["://"];

    public static void SetIdentityServerOrigin(this HttpContext context, string value)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(value);

        var split = value.Split(_separator, StringSplitOptions.RemoveEmptyEntries);

        var request = context.Request;
        request.Scheme = split.First();
        request.Host = new HostString(split.Last());
    }

    public static void SetIdentityServerBasePath(this HttpContext context, string value)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Items[Constants.EnvironmentKeys.IdentityServerBasePath] = value;
    }

    public static string GetIdentityServerOrigin(this HttpContext context)
    {
        var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
        var request = context.Request;
        
        if (options.MutualTls.Enabled && options.MutualTls.DomainName.IsPresent())       
            if (!options.MutualTls.DomainName.Contains('.'))            
                if (request.Host.Value.StartsWith(options.MutualTls.DomainName, StringComparison.OrdinalIgnoreCase))              
                    return string.Concat(request.Scheme, "://", request.Host.Value.AsSpan(options.MutualTls.DomainName.Length + 1));
                       
        return request.Scheme + "://" + request.Host.Value;
    }


    internal static void SetSignOutCalled(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Items[Constants.EnvironmentKeys.SignOutCalled] = "true";
    }

    internal static bool GetSignOutCalled(this HttpContext context)
    {
        return context.Items.ContainsKey(Constants.EnvironmentKeys.SignOutCalled);
    }

    /// <summary>
    /// Gets the host name of IdentityServer.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public static string GetIdentityServerHost(this HttpContext context)
    {
        var request = context.Request;
        return request.Scheme + "://" + request.Host.ToUriComponent();
    }

    /// <summary>
    /// Gets the base path of IdentityServer.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public static string GetIdentityServerBasePath(this HttpContext context)
    {
        return context.Items[Constants.EnvironmentKeys.IdentityServerBasePath] as string;
    }

    /// <summary>
    /// Gets the public base URL for IdentityServer.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public static string GetIdentityServerBaseUrl(this HttpContext context)
    {
        var baseUrl = context.GetIdentityServerHost();

        UseGatewayAsIdentity(context, ref baseUrl);

        return baseUrl + context.GetIdentityServerBasePath();
    }

    /// <summary>
    /// Gets the identity server relative URL.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    public static string GetIdentityServerRelativeUrl(this HttpContext context, string path)
    {
        if (!path.IsLocalUrl())   
            return null;
        
        if (path.StartsWith("~/"))
            path = path[1..];
        
        path = context.GetIdentityServerBaseUrl().EnsureTrailingSlash() + path.RemoveLeadingSlash();
        
        return path;
    }

    /// <summary>
    /// Gets the identity server issuer URI.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">context</exception>
    public static string GetIdentityServerIssuerUri(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        // if they've explicitly configured a URI then use it,
        // otherwise dynamically calculate it
        var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
        var uri = options.IssuerUri;
        if (uri.IsMissing())
        {
            uri = context.GetIdentityServerOrigin() + context.GetIdentityServerBasePath();
            if (uri.EndsWith('/'))
                uri = uri[..^1];

            if (options.LowerCaseIssuerUri)
                uri = uri.ToLowerInvariant();
        }

        UseGatewayAsIdentity(context, ref uri);

        return uri;     
    }

    internal static async Task<string> GetIdentityServerSignoutFrameCallbackUrlAsync(this HttpContext context, LogoutMessage logoutMessage = null)
    {
        var userSession = context.RequestServices.GetRequiredService<IUserSession>();
        var user = await userSession.GetUserAsync();
        var currentSubId = user?.GetSubjectId();

        LogoutNotificationContext endSessionMsg = null;

        // if we have a logout message, then that take precedence over the current user
        if (logoutMessage?.ClientIds?.Any() == true)
        {
            var clientIds = logoutMessage?.ClientIds;

            // check if current user is same, since we might have new clients (albeit unlikely)
            if (currentSubId == logoutMessage?.SubjectId)
            {
                clientIds = clientIds.Union(await userSession.GetClientListAsync());
                clientIds = clientIds.Distinct();
            }

            endSessionMsg = new LogoutNotificationContext
            {
                SubjectId = logoutMessage.SubjectId,
                SessionId = logoutMessage.SessionId,
                ClientIds = clientIds
            };
        }
        else if (currentSubId != null)
        {
            // see if current user has any clients they need to signout of 
            var clientIds = await userSession.GetClientListAsync();
            if (clientIds.Any())
            {
                endSessionMsg = new LogoutNotificationContext
                {
                    SubjectId = currentSubId,
                    SessionId = await userSession.GetSessionIdAsync(),
                    ClientIds = clientIds
                };
            }
        }

        if (endSessionMsg != null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var clock = context.RequestServices.GetRequiredService<ISystemClock>();
#pragma warning restore CS0618 // Type or member is obsolete
            var msg = new Message<LogoutNotificationContext>(endSessionMsg, clock.UtcNow.UtcDateTime);

            var endSessionMessageStore = context.RequestServices.GetRequiredService<IMessageStore<LogoutNotificationContext>>();
            var id = await endSessionMessageStore.WriteAsync(msg);

            var signoutIframeUrl = context.GetIdentityServerBaseUrl().EnsureTrailingSlash() + Constants.ProtocolRoutePaths.EndSessionCallback;
            signoutIframeUrl = signoutIframeUrl.AddQueryString(Constants.UIConstants.DefaultRoutePathParams.EndSessionCallback, id);

            return signoutIframeUrl;
        }

        // no sessions, so nothing to cleanup
        return null;
    }

    private static void UseGatewayAsIdentity(HttpContext context, ref string uri)
    {
        var useGatewayAsIdentity = context.Request.Headers["Use-Gateway-As-Identity"].FirstOrDefault();
        if (useGatewayAsIdentity is "true")
        {
            var gatewayUrl = context.Request.Headers["Gateway-Url"].FirstOrDefault();
            if (gatewayUrl is not null)
                uri = gatewayUrl;
        }
    }
}

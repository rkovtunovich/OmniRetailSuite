﻿using Identity.Api.Infrastructure.Data;
using Identity.Api.Models;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Configuration;

public static class ConfigureIdentityServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
        {
            options.IssuerUri = configuration["baseUrls:webBase"];
            options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(GetIdentityResources())
            .AddInMemoryApiScopes(GetApiScopes())
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryClients(GetClients(configuration))
            .AddAspNetIdentity<ApplicationUser>();

        services.AddLocalApiAuthentication();

        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy.WithOrigins(configuration["BlazorWasmClient"] ?? throw new ArgumentNullException("BlazorWasmClient"))
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    private static List<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    private static List<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
             new("api", "Api: catalog, basket"),
             new("backoffice", "BackOffice"),
             new("retail.client.wasm", "Retail client wasm"),
             new("web", "Web eShop"),
             new(IdentityServerConstants.LocalApi.ScopeName, "Identity server API"),
             new("webappsgateway", "WEB Apps API Gateway")
                    {
                        ApiSecrets = { new Secret("webappsgateway-secret".Sha256()) },
                        Scopes = { "webappsgateway" }                       
                    }
        };
    }

    private static List<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
                new("api", "Api: catalog, basket"),
                new("backoffice", "BackOffice"),
                new("retail.client.wasm", "Retail client wasm"),
                new("web", "Web eShop"),
                new(IdentityServerConstants.LocalApi.ScopeName, "Identity server api"),
                new("webappsgateway", "WEB Apps API Gateway")
        };
    }

    private static List<Client> GetClients(IConfiguration configuration)
    {
        return new List<Client>
        {
                new() {
                    ClientId = "blazorOpenId",
                    ClientName = "Blazor Admin OpenId Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,                    
                    //Used to retrieve the access token on the back channel.
                    ClientSecrets =
                    {
                        new Secret("blazorSecret".Sha256())
                    },
                    //RedirectUris = { "http://blazoradmin" },
                    //RequireConsent = false,
                    //RequirePkce = true,
                    //PostLogoutRedirectUris = { $"http://blazoradmin/Account/Redirecting" },
                    //AllowedCorsOrigins = { "http://eshopxamarin" },
                    AllowedScopes = new List<string>
                    {
                        //IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        "webappsgateway",
                         IdentityServerConstants.LocalApi.ScopeName
                    },
                    //Allow requesting refresh tokens for long lived API access
                    //AllowOfflineAccess = true,
                    //AllowAccessTokensViaBrowser = true
                },
                new() {
                    ClientId = "blazorInteractive",
                    ClientName = "Blazor admin interactive client",
                    ClientSecrets = new List<Secret>
                    {
                        new("blazorSecret".Sha256())
                    },
                    ClientUri = $"{configuration["BlazorAdminClient"]}/", // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    //AllowAccessTokensViaBrowser = false,
                    //RequireConsent = false,
                    //RequireConsent = true,
                    //RequirePkce = false,
                    AllowPlainTextPkce = true,
                    AllowOfflineAccess = true,
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string>
                    {
                        $"{configuration["BlazorAdminClient"]}/signin-oidc"
                    },
                    FrontChannelLogoutUri = $"{configuration["BlazorAdminClient"]}/signout-oidc",
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{configuration["BlazorAdminClient"]}/signout-callback-oidc",
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,                     
                        IdentityServerConstants.LocalApi.ScopeName,
                        "webappsgateway"
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime= 60 * 60 * 2 // 2 hours
                },
                new()
                {
                    ClientId = "blazorWasm",
                    ClientName = "Blazor wasm client",
                    ClientUri = $"{configuration["BlazorWasmClient"]}/", // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    //AllowAccessTokensViaBrowser = false,
                    //RequireConsent = false,
                    //RequireConsent = true,
                    //RequirePkce = false,
                    AllowPlainTextPkce = true,
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = { configuration["BlazorWasmClient"] },
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string>
                    {
                        $"{configuration["BlazorWasmClient"]}/authentication/login-callback"
                    },
                    FrontChannelLogoutUri = $"{configuration["BlazorWasmClient"]}/authentication/logout-callback",
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{configuration["BlazorWasmClient"]}/authentication/logout-callback",
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime= 60 * 60 * 2 // 2 hours
                },
                new() {
                    ClientId = "web",
                    ClientName = "Web eShop Client",
                    ClientSecrets = new List<Secret>
                    {

                        new("secret".Sha256())
                    },
                    ClientUri = $"{"http://eshopwebmvc"}",                             // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequirePkce = false,
                    RedirectUris = new List<string>
                    {
                        $"http://eshopwebmvc/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"http://eshopwebmvc/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                    },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime= 60 * 60 * 2 // 2 hours
                }
        };
    }
}

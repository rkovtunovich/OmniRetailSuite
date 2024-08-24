using Identity.Api.Infrastructure.Data;
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
            options.IssuerUri = configuration["IssuerUri"];
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
                policy.WithOrigins(configuration["RetailAssistant"] ?? throw new NullReferenceException("RetailAssistant"))
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    private static List<IdentityResource> GetIdentityResources()
    {
        return
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];
    }

    private static List<ApiResource> GetApiResources()
    {
        return
        [
             new("api", "Api: catalog, retail"),
             new("backoffice", "BackOffice"),
             new("retail.client.wasm", "Retail client wasm"),
             new(IdentityServerConstants.LocalApi.ScopeName, "Identity server API"),
             new("webappsgateway", "WEB Apps API Gateway")
                    {
                        ApiSecrets = { new Secret("webappsgateway-secret".Sha256()) },
                        Scopes = { "webappsgateway" }
                    }
        ];
    }

    private static List<ApiScope> GetApiScopes()
    {
        return
        [
                new("api", "Api: catalog, retail"),
                new("backoffice", "BackOffice"),
                new(IdentityServerConstants.LocalApi.ScopeName, "Identity server api"),
                new("webappsgateway", "WEB Apps API Gateway")
        ];
    }

    private static List<Client> GetClients(IConfiguration configuration)
    {
        return
        [
                new() {
                    ClientId = "blazorOpenId",
                    ClientName = "Blazor Admin OpenId Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,                     
                    ClientSecrets =
                    {
                        new Secret("blazorSecret".Sha256())
                    },
                    AllowedScopes =
                    [
                        "webappsgateway",
                         IdentityServerConstants.LocalApi.ScopeName
                    ],
                },
                new() {
                    ClientId = "blazorInteractive",
                    ClientName = "Blazor admin interactive client",
                    ClientSecrets =
                    [
                        new("blazorSecret".Sha256())
                    ],
                    ClientUri = $"{configuration["Backoffice"]}/", // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowPlainTextPkce = true,
                    AllowOfflineAccess = true,
                    RedirectUris =
                    [
                        $"{configuration["Backoffice"]}/signin-oidc"
                    ],
                    FrontChannelLogoutUri = $"{configuration["Backoffice"]}/signout-oidc",
                    PostLogoutRedirectUris =
                    [
                        $"{configuration["Backoffice"]}/signout-callback-oidc",
                    ],
                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        "webappsgateway"
                    ],
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime= 60 * 60 * 2 // 2 hours
                },
                new()
                {
                    ClientId = "blazorWasm",
                    ClientName = "Blazor wasm client",
                    ClientUri = $"{configuration["RetailAssistant"]}/", // public uri of the client
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = { configuration["RetailAssistant"] },
                    AccessTokenLifetime = 60 * 60 * 2, // 2 hours
                    IdentityTokenLifetime = 60 * 60 * 2, // 2 hours
                    AbsoluteRefreshTokenLifetime = 60 * 60 * 24 * 30, // 30 days
                    SlidingRefreshTokenLifetime = 60 * 60 * 24 * 15, // 15 days
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RedirectUris =
                    [
                        $"{configuration["RetailAssistant"]}/authentication/login-callback"
                    ],
                    FrontChannelLogoutUri = $"{configuration["RetailAssistant"]}/authentication/logout-callback",
                    PostLogoutRedirectUris =
                    [
                        $"{configuration["RetailAssistant"]}/authentication/logout-callback",
                    ],
                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api",
                        IdentityServerConstants.LocalApi.ScopeName,
                        "webappsgateway"
                    ]
                }
        ];
    }
}

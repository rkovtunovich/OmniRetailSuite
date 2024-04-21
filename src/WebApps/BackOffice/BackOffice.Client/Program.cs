using BackOffice.Application.Configuration;
using BackOffice.Client.Configuration;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using MudExtensions.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

if (builder.Environment.IsDevelopment())
    IdentityModelEventSource.ShowPII = true;

builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddJsonTextSerialization();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(config => config.DetailedErrors = false);

builder.Services.AddScoped<ToastService>();
builder.Services.AddSingleton<TabsService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.Authority = builder.Configuration.GetValue<string>("IdentityServerSettings:Authority");
        options.ClientId = builder.Configuration.GetValue<string>("InteractiveServiceSettings:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("InteractiveServiceSettings:ClientSecret");
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
        options.UseSecurityTokenValidator = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
           ValidateIssuer = false
        };
    });

builder.Services.AddAppServices();
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
builder.Services.AddRadzenComponents();

builder.Logging.AddConfiguration(builder.Configuration.GetRequiredSection("Logging"));

var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.SameAsRequest
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();

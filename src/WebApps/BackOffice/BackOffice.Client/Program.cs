using BackOffice.Application.Configuration;
using BackOffice.Client.Configuration;
using BackOffice.Client.Endpoints;
using BackOffice.Client.Localization;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using MudExtensions.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.untracked.json", optional: true, reloadOnChange: true);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

if (builder.Environment.IsDevelopment())
    IdentityModelEventSource.ShowPII = true;

builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddJsonTextSerialization();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<TabsService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped<MudLocalizer, BackofficeLocalizer>();

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

builder.Logging.AddConfiguration(builder.Configuration.GetRequiredSection("Logging"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(SupportedCultures.Default)
    .AddSupportedCultures(SupportedCultures.All)
    .AddSupportedUICultures(SupportedCultures.All);
app.UseRequestLocalization(localizationOptions);

app.MapCultureChangingEndpoint();
app.MapAuthenticationEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();

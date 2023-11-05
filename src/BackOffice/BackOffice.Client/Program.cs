using BackOffice.Client.Configuration;
using BackOffice.Client.Model.Options;
using BackOffice.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using MudBlazor.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

var configSection = builder.Configuration.GetRequiredSection(BaseUrlConfiguration.CONFIG_NAME);
builder.Services.Configure<BaseUrlConfiguration>(configSection);
builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();

builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(config => config.DetailedErrors = false);

builder.Services.AddScoped<ToastService>();

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
        options.Authority = builder.Configuration.GetValue<string>("WebGateway");
        options.ClientId = builder.Configuration.GetValue<string>("InteractiveServiceSettings:ClientId");
        options.ClientSecret = builder.Configuration.GetValue<string>("InteractiveServiceSettings:ClientSecret");
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddBackOfficeServices();
builder.Services.AddMudServices();
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

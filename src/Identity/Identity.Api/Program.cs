using Identity.Api.Configuration;
using Identity.Api.Infrastructure.Data;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Steeltoe.Discovery.Client;
using Winton.Extensions.Configuration.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConsul($"configuration/identity/{builder.Environment.EnvironmentName.ToLower()}");

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel")); 
});

if (builder.Environment.IsDevelopment())
    IdentityModelEventSource.ShowPII = true;

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});

builder.Services.AddControllersWithViews();
builder.Services.AddDataManagement(builder.Configuration);
builder.Services.AddIdentityDbContext(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddUserPreferences();
builder.Services.AddSecretManagement(builder.Configuration);

var app = builder.Build();

await DbSeeder.SeedDatabase(app);

app.UseHttpsRedirection(); 

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.SameAsRequest
});
app.UseCors("default"); 
app.UseRouting();
app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

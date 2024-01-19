using Identity.Api.Configuration;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

// TO DO 
// only for dev
IdentityModelEventSource.ShowPII = true;

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders;
});

builder.Services.AddControllersWithViews();
builder.Services.AddIdentityDbContext(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddUserPreferences();

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

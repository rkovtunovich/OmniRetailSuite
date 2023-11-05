using Consul;
using IdentityServer4.AccessTokenValidation;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using WebAppsGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot().AddConsul();

// Extract Consul Settings
var consulHost = builder.Configuration["GlobalConfiguration:ServiceDiscoveryProvider:Host"];
var consulPort = int.Parse(builder.Configuration["GlobalConfiguration:ServiceDiscoveryProvider:Port"] ?? throw new ConsulConfigurationException("The Consul port isn't defined"));

// Register the Consul client
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
   consulConfig.Address = new Uri($"http://{consulHost}:{consulPort}");
}));

builder.Services.AddAuthentication("IdentityServer")
       .AddIdentityServerAuthentication("IdentityServer", options =>
       {
           options.Authority = null; // Set in middleware
           options.RequireHttpsMetadata = false; // Set to true in production
           options.ApiName = "webappsgateway";
           options.ApiSecret = "webappsgateway-secret";
           options.SupportedTokens = SupportedTokens.Both;
       });

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseMiddleware<UpdateIdentityServerAuthorityMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();

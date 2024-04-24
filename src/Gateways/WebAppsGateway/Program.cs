using Consul;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using WebAppsGateway.Configuration;
using WebAppsGateway.DelegatingHandlers;
using WebAppsGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddHealthChecks();
builder.Services.AddOcelot()
    .AddConsul()
    .AddDelegatingHandler<GatewayHeadersDelegationHandler>();

builder.Services.Configure<GatewaySettings>(builder.Configuration.GetRequiredSection(nameof(GatewaySettings)));
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Extract Consul Settings
var consulHost = builder.Configuration["GlobalConfiguration:ServiceDiscoveryProvider:Host"];
var consulPort = int.Parse(builder.Configuration["GlobalConfiguration:ServiceDiscoveryProvider:Port"] ?? throw new ConsulConfigurationException("The Consul port isn't defined"));

// Register the Consul client
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
   consulConfig.Address = new Uri($"http://{consulHost}:{consulPort}");
}));

builder.Services.AddIdentityServer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCors("default");

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

app.UseMiddleware<UpdateIdentityServerAuthorityMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/_health" ,new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.UseOcelot();

app.Run();

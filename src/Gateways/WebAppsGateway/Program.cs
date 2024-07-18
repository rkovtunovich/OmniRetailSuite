using Consul;
using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using WebAppsGateway.Configuration;
using WebAppsGateway.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.untracked.json", optional: true, reloadOnChange: true);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddAppHealthChecks();
builder.Services.AddOcelot()
    .AddConsul();

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
builder.Services.AddJsonTextSerialization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCors("default");

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/_health" ,new HealthCheckOptions
{
    ResponseWriter = HealthCheckResponseWriter.WriteResponse
});

await app.UseOcelot();

app.Run();

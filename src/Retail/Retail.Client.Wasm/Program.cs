using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Retail.Client.Wasm;
using Retail.Client.Wasm.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddMudServices();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
    //options.ProviderOptions.Authority = builder.Configuration.GetValue<string>("IdentityUrlExternal");
    //options.ProviderOptions.ClientId = builder.Configuration.GetValue<string>("InteractiveServiceSettings:ClientId");
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("api");
    options.ProviderOptions.ResponseType = "code";
});

var app = builder.Build();

await app.RunAsync();

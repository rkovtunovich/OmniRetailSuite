using Infrastructure.Serialization.JsonText.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using MudExtensions.Services;
using RetailAssistant.Application.Config;
using RetailAssistant.Client.Configuration;
using RetailAssistant.Data.Configuration;
using UI.Razor.Services.Implementation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Configuration.AddJsonFile("appsettings.untracked.json", optional: true, reloadOnChange: true);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddJsonTextSerialization();
builder.Services.AddRetailAssistantWebServices(builder.Configuration);
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
builder.Services.AddRetailAssistantAppServices();
builder.Services.AddIndexedDb();
builder.Services.AddSingleton<TabsService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);

    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("api");
    options.ProviderOptions.DefaultScopes.Add("webappsgateway");
    options.ProviderOptions.DefaultScopes.Add("IdentityServerApi");
    options.ProviderOptions.ResponseType = "code";
});

builder.Services.AddScoped<AuthenticationStateProvider, PersistentRemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount>>();

var app = builder.Build();

try
{
    var jsRuntime = app.Services.GetRequiredService<IJSRuntime>();
    // This is a workaround for the issue with the service worker not updating the cache after the first load
    await jsRuntime.InvokeVoidAsync("checkForServiceWorkerUpdate");

    const string defaultCulture = "en";

    var result = await jsRuntime.InvokeAsync<string>("blazorCulture.get");
    var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

    if (result is null)
        await jsRuntime.InvokeVoidAsync("blazorCulture.set", defaultCulture);

    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;

    await app.ConfigureIndexedDbAsync();
    app.StartDataSynchronization();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

await app.RunAsync();

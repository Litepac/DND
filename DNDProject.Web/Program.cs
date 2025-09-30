using System.Net.Http;
using DNDProject.Web;
using DNDProject.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Client-side auth (kræver Microsoft.AspNetCore.Components.Authorization)
builder.Services.AddAuthorizationCore();

// Tokenlager + delegating handler
builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddScoped<TokenAuthorizationMessageHandler>();

// Navngiven HttpClient til API'et (med handleren der sætter Authorization-header)
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5230/");
})
.AddHttpMessageHandler<TokenAuthorizationMessageHandler>();

// Standard HttpClient -> brug den navngivne "Api"
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

await builder.Build().RunAsync();

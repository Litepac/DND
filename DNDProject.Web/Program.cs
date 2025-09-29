using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;
using DNDProject.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<DNDProject.Web.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient peger på API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5230/")  // API-port
});

builder.Services.AddAuthorizationCore();

// Token service
builder.Services.AddScoped<TokenStorage>();

// Læs token ved app-start og sæt Authorization-header
var host = builder.Build();

var tokens = host.Services.GetRequiredService<TokenStorage>();
var http   = host.Services.GetRequiredService<HttpClient>();

var existing = await tokens.GetAsync();
if (!string.IsNullOrWhiteSpace(existing))
{
    http.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", existing);
}

await host.RunAsync();

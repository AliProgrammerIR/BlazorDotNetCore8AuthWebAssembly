using BlazorDocManagerDotNet8.Client;
using BlazorDocManagerDotNet8.Client.Auth;
using BlazorDocManagerDotNet8.Client.Helpers;
using BlazorDocManagerDotNet8.Client.Repositories;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<JWTAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>
	(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());

builder.Services.AddScoped<ILoginService, JWTAuthenticationStateProvider>
	(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();

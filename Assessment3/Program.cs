using Assessment3;
using Assessment3.Models;
using Assessment3.Services;
using Assessment3.Services.Implementation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("Debug", client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["ConnectionStrings:ApiUri"]!);
    client.BaseAddress = new Uri("https://localhost:7189");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
//Dependenices
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddSingleton<Shared>();
await builder.Build().RunAsync();

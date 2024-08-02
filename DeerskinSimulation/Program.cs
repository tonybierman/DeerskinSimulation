using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DeerskinSimulation;
using DeerskinSimulation.Models;
using DeerskinSimulation.Services;
using DeerskinSimulation.ViewModels;
using DeerskinSimulation.Commands;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// User Services
builder.Services.AddSingleton<IStateContainer, StateContainer>();
builder.Services.AddSingleton<IGameLoopService, GameLoopService>();
builder.Services.AddScoped<ICommandFactory, CommandFactory>();
builder.Services.AddScoped<ISimulationViewModel, SimulationViewModel>();

await builder.Build().RunAsync();

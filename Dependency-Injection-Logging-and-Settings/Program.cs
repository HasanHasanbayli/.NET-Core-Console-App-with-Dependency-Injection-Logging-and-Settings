using Dependency_Injection_Logging_and_Settings.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
    {
        services.AddScoped<IProductService, ProductService>();
    })
    .UseSerilog()
    .Build();

// Ask the service provider for the configuration abstraction.
var configuration = host.Services.GetRequiredService<IConfiguration>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration.GetSection(""))
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger.Information("Application starting");

// Get values from the config given their key and their target type.
var keyOneValue = configuration.GetValue<int>("KeyOne");
var keyTwoValue = configuration.GetValue<bool>("KeyTwo");
var keyThreeNestedValue = configuration.GetValue<string>("KeyThree:Message");

// Write the values to the console.
Console.WriteLine($"KeyOne = {keyOneValue}");
Console.WriteLine($"KeyTwo = {keyTwoValue}");
Console.WriteLine($"KeyThree:Message = {keyThreeNestedValue}");

var svc = host.Services.GetRequiredService<IProductService>();

svc.DoSomething();
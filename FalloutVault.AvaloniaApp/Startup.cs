using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.Eventing;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FalloutVault.AvaloniaApp;

public static class Startup
{
    public static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        AddViewModels(services);

        services
            .AddSingleton<ILogger>(provider =>
                new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger());

        services
            .AddSingleton<IEventBus<DeviceMessage>, DeviceMessageEventBus>()
            .AddSingleton<IEventBus<Watt>, PowerEventBus>()
            .AddSingleton<IDeviceController, DeviceController>()
            .AddSingleton<IDeviceRegistry, DeviceRegistry>();

        return services;
    }

    private static IServiceCollection AddViewModels(IServiceCollection services)
    {
        services
            .AddTransient<MainWindowViewModel>();

        return services;
    }
}
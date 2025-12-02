using FalloutVault.AvaloniaApp.Services;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.AvaloniaApp.Views;
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
                    .WriteTo.File("log.txt")
                    .CreateLogger());

        services
            .AddSingleton<IEventBus<DeviceMessage>, DeviceMessageEventBus>()
            .AddSingleton<IEventBus<Watt>, PowerEventBus>()
            .AddSingleton<IDeviceController, DeviceController>()
            .AddSingleton<IDeviceRegistry, DeviceRegistry>()
            .AddSingleton<IDeviceMessageLogger, DeviceMessageLogger>()
            .AddSingleton<MainWindow>();

        return services;
    }

    private static IServiceCollection AddViewModels(IServiceCollection services)
    {
        services
            .AddTransient<MainWindowViewModel>();

        services
            .AddKeyedTransient<IDeviceViewModel, LightControllerViewModel>(DeviceType.LightController)
            .AddKeyedTransient<IDeviceViewModel, FanControllerViewModel>(DeviceType.FanController)
            .AddKeyedTransient<IDeviceViewModel, SpeakerControllerViewModel>(DeviceType.SpeakerController)
            .AddKeyedTransient<IDeviceViewModel, PowerControllerViewModel>(DeviceType.PowerController)
            .AddKeyedTransient<IDeviceViewModel, CropSprinklerControllerViewModel>(DeviceType.CropSprinklerController)
            .AddKeyedTransient<IDeviceViewModel, VentSealControllerViewModel>(DeviceType.VentSealController)
            .AddSingleton<DeviceViewModelFactory>();

        return services;
    }
}
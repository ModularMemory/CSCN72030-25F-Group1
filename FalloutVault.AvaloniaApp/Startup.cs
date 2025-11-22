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
                    .CreateLogger());

        services
            .AddSingleton<IEventBus<DeviceMessage>, DeviceMessageEventBus>()
            .AddSingleton<IEventBus<Watt>, PowerEventBus>()
            .AddSingleton<IDeviceController, DeviceController>()
            .AddSingleton<IDeviceRegistry, DeviceRegistry>()
            .AddSingleton<MainWindow>();


        return services;


    }

    private static IServiceCollection AddViewModels(IServiceCollection services)
    {
        services
            .AddTransient<MainWindowViewModel>();

        services
            .AddKeyedTransient<IDeviceViewModel, LightControllerViewModel>(DeviceType.LightController)
            .AddKeyedTransient<IDeviceViewModel, LightControllerViewModel>(DeviceType.FanController)
            .AddKeyedTransient<IDeviceViewModel, SpeakerControllerViewModel>(DeviceType.SpeakerController)
            .AddKeyedTransient<IDeviceViewModel, LightControllerViewModel>(DeviceType.PowerController)
            .AddSingleton<DeviceViewModelFactory>(ctx =>
            {
                var vms = Enum.GetValues<DeviceType>()
                    .Where(x => x != DeviceType.Unknown)
                    .Select(type => (type, ctx.GetRequiredKeyedService<IDeviceViewModel>(type)));

                var dict = new Dictionary<DeviceType, Func<IDeviceViewModel>>();

                foreach (var (type, vm) in vms)
                {
                    dict.Add(type, () => ctx.GetRequiredKeyedService<IDeviceViewModel>(type));
                }

                return new DeviceViewModelFactory(dict);
            });

        return services;
    }
}
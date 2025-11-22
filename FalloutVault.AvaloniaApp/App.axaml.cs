using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using Avalonia.Markup.Xaml;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.AvaloniaApp.Views;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FalloutVault.AvaloniaApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }


    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        var serviceProvider = Startup.ConfigureServices(services)
            .BuildServiceProvider();

        AddDevices(serviceProvider.GetService<IDeviceRegistry>()!);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
            };
        }


        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private void AddDevices(IDeviceRegistry deviceRegistry)
    {
        IEnumerable<IDevice> devices =
        [
            // Lights
            new LightController(new DeviceId("Light-1", "East Hall"), (Watt)50),
            new LightController(new DeviceId("Light-2", "East Hall"), (Watt)100),
            new LightController(new DeviceId("Light-3", "East Hall"), (Watt)75),
            new LightController(new DeviceId("Light-4", "West Hall"), (Watt)50),
            new LightController(new DeviceId("Light-5", "North Hall"), (Watt)120),
            // Fans
            new FanController(new DeviceId("Fan-1", "East Hall")),
            new FanController(new DeviceId("Fan-3", "West Hall")),
            new FanController(new DeviceId("Fan-2", "North Hall")),
            // Speaker controller
            new SpeakerController(new DeviceId("Speaker-1", "East Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-2", "West Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-3", "North Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-4", "Generator Room"), (Watt)100),
            // Power Controller
            new PowerController(new DeviceId("Central-Reactor", "Generator Room"), (Watt)1_000)
        ];
        foreach (var device in devices)
        {
            deviceRegistry.RegisterDevice(device);
        }
    }
}
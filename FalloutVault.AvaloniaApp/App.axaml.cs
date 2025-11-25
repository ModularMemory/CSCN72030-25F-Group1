using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.AvaloniaApp.Views;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FalloutVault.AvaloniaApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private static ILogger _logger = null!;
    private static void MessageBusOnMessage(object? sender, DeviceMessage e)
    {

        var senderString = (sender as IDevice)?.Id.ToString() ?? sender?.ToString();
        _logger.Information("Device message from {Sender}: {@Message}", senderString, e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()
    }
    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        var serviceProvider = Startup.ConfigureServices(services)
            .BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger>();
        serviceProvider.GetRequiredService<IEventBus<DeviceMessage>>().Handler += MessageBusOnMessage;
        AddDevices(serviceProvider.GetService<IDeviceRegistry>()!);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = serviceProvider.GetRequiredService<MainWindow>();

            desktop.MainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();

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

    private static void AddDevices(IDeviceRegistry deviceRegistry)
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
            new FanController(new DeviceId("Fan-1", "East Hall"), (Watt)50, 2_000),
            new FanController(new DeviceId("Fan-3", "West Hall"), (Watt)75, 1_500),
            new FanController(new DeviceId("Fan-2", "North Hall"), (Watt)100, 4_000),
            // Speaker controller
            new SpeakerController(new DeviceId("Speaker-1", "East Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-2", "West Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-3", "North Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-4", "Generator Room"), (Watt)100),
            // Crop sprinkler controller
            new CropSprinklerController(new DeviceId("CropSprinkler-1", "East Field")),
            new CropSprinklerController(new DeviceId("CropSprinkler-2", "West Field")),
            // Vent seal controller
            new VentSealController(new DeviceId("VentSeal-1", "East Hall")),
            new VentSealController(new DeviceId("VentSeal-2", "West Hall")),
            new VentSealController(new DeviceId("VentSeal-3", "North Hall")),
            new VentSealController(new DeviceId("VentSeal-4", "South Hall")),
            // Power Controller
            new PowerController(new DeviceId("Central-Reactor", "Generator Room"), (Watt)1_000)
        ];

        foreach (var device in devices)
        {
            deviceRegistry.RegisterDevice(device);
        }
    }
}
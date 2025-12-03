using System.Diagnostics;
using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FalloutVault.ConsoleApp;

internal static class Program
{
    private static ILogger _logger = null!;

    public static async Task Main(string[] args)
    {
        // Setup services
        var serviceCollection = new ServiceCollection();

        var serviceProvider = AddServices(serviceCollection)
            .BuildServiceProvider();

        _logger = serviceProvider.GetRequiredService<ILogger>();
        serviceProvider.GetRequiredService<IEventBus<DeviceMessage>>().Handler += MessageBusOnMessage;

        // Add devices
        var registry = serviceProvider.GetRequiredService<IDeviceRegistry>();
        AddDevices(registry);

        // Run
        var controller = serviceProvider.GetRequiredService<IDeviceController>();
        controller.Start();

        await ModifyDevices(controller, registry, TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(250));

        controller.Stop();
    }

    private static IServiceCollection AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<ILogger>(provider =>
                new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger());

        serviceCollection
            .AddSingleton<IEventBus<DeviceMessage>, DeviceMessageEventBus>()
            .AddSingleton<IEventBus<Watt>, PowerEventBus>()
            .AddSingleton<IDeviceRegistry, DeviceRegistry>()
            .AddSingleton<IDeviceController, DeviceController>();

        return serviceCollection;
    }

    private static void MessageBusOnMessage(object? sender, DeviceMessage e)
    {
        var senderString = (sender as IDevice)?.Id.ToString() ?? sender?.ToString();
        _logger.Information("Device message from {Sender}: {@Message}", senderString, e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()
    }

    private static async Task ModifyDevices(IDeviceController controller, IDeviceRegistry registry, TimeSpan runTime, TimeSpan deviceModifyDelay)
    {
        var random = new Random(69);
        var sw = Stopwatch.StartNew();

        var deviceInfos = registry.Devices
            .Select(x => (x.id, registry[x.id].type, registry[x.id].capabilities))
            .ToArray();

        while (sw.Elapsed < runTime)
        {
            // Sleep
            await Task.Delay(deviceModifyDelay);

            // Get a device
            var device = random.GetItems(deviceInfos, 1).First();

            // Modify it (capabilities)
            if (device.capabilities.HasFlag(DeviceCapabilities.OnOff))
            {
                var isOn = random.Next(0, 2) == 0;
                controller.SendCommand(device.id, new DeviceCommand.SetOn(isOn));
            }

            // Modify it (type)
            switch (device.type)
            {
                case DeviceType.LightController:
                    var dimmerLevel = Math.Sqrt(random.NextDouble()); // sqrt to bias towards higher dimmer levels
                    controller.SendCommand(device.id, new DeviceCommand.SetLightDimmer(dimmerLevel));
                    break;
                case DeviceType.FanController:
                    var targetRpm = random.Next(0, 2_000);
                    controller.SendCommand(device.id, new DeviceCommand.SetFanTargetRpm(targetRpm));
                    break;
                case DeviceType.SpeakerController:
                    var volume = random.NextDouble();
                    controller.SendCommand(device.id, new DeviceCommand.SetSpeakerVolume(volume));
                    break;
                case DeviceType.CropSprinklerController:
                    var targetSection = random.Next(0, 4);
                    controller.SendCommand(device.id, new DeviceCommand.CurrentCropSection(targetSection));
                    break;
                case DeviceType.VentSealController:
                    var open = random.Next(0, 2) == 0;
                    controller.SendCommand(device.id, new DeviceCommand.SetVentOpen(open));
                    var locked = random.Next(0, 5) == 0;
                    controller.SendCommand(device.id, new DeviceCommand.SetVentLocked(locked));
                    break;
                case DeviceType.PowerController:
                    controller.SendCommand(device.id, new DeviceCommand.SetOn(true));
                    controller.SendCommand(device.id, new DeviceCommand.SetOn(false));
                    break;
            }
        }
    }

    private static void AddDevices(IDeviceRegistry registry)
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
            registry.RegisterDevice(device);
        }
    }
}
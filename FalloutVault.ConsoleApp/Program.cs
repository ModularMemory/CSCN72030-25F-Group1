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
        var devices = GetDevices();
        foreach (var device in devices)
        {
            registry.RegisterDevice(device);
        }

        // Run
        var controller = serviceProvider.GetRequiredService<IDeviceController>();
        controller.Start();

        await ModifyDevices(devices, controller, registry, TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(250));

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
        _logger.Information("Device message: {@Message}", e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()
    }

    private static async Task ModifyDevices(IReadOnlyList<IDevice> devices, IDeviceController controller, IDeviceRegistry registry, TimeSpan runTime, TimeSpan deviceModifyDelay)
    {
        var random = new Random(69);
        var sw = Stopwatch.StartNew();

        var deviceInfos = devices
            .Select(x => (x.Id, registry[x.Id].type, registry[x.Id].capabilities))
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
                controller.SendCommand(device.Id, new DeviceCommand.SetOn(isOn));
            }

            // Modify it (type)
            switch (device.type)
            {
                case DeviceType.LightController:
                    var dimmerLevel = Math.Sqrt(random.NextDouble()); // sqrt to bias towards higher dimmer levels
                    controller.SendCommand(device.Id, new DeviceCommand.SetLightDimmer(dimmerLevel));
                    break;
                case DeviceType.FanController:
                    // TODO: Convert to a command:
                    // fanController.TargetRpm = random.Next(0, 2_000);
                    break;
                case DeviceType.SpeakerController:
                    break;
                case DeviceType.PowerController:
                    // TODO:
                    break;
            }
        }
    }

    private static IReadOnlyList<IDevice> GetDevices()
    {
        return
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
    }
}
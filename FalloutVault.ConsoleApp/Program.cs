using System.Diagnostics;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace FalloutVault.ConsoleApp;

internal static class Program
{
    private static Logger _logger = null!;

    public static async Task Main(string[] args)
    {
        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var registry = new DeviceRegistry(_logger);
        using var controller = new DeviceController(registry, _logger);

        controller.MessageBus.Handler += MessageBusOnMessage;
        var devices = GetDevices();

        foreach (var device in devices)
        {
            controller.AddDevice(device);
        }

        controller.Start();

        await ModifyDevices(devices, TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(250));

        controller.Stop();
    }

    private static void MessageBusOnMessage(object? sender, DeviceMessage e)
    {
        _logger.Information("Device message: {@Message}", e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()
    }

    private static async Task ModifyDevices(IReadOnlyList<IDevice> devices, TimeSpan runTime, TimeSpan deviceModifyDelay)
    {
        var random = new Random(69);
        var sw = Stopwatch.StartNew();

        while (sw.Elapsed < runTime)
        {
            // Sleep
            await Task.Delay(deviceModifyDelay);

            // Get a random device
            var device = random.GetItems(devices.ToArray(), 1).First();

            // Do something to it
            switch (device)
            {
                case LightController lightController:
                    lightController.IsOn = random.Next(0, 2) == 0;
                    lightController.DimmerLevel = Math.Sqrt(random.NextDouble()); // sqrt to bias towards higher dimmer levels
                    break;
                case FanController fanController:
                    fanController.IsOn = random.Next(0, 2) == 0;
                    fanController.TargetRpm = random.Next(0, 2_000);
                    break;
                case SpeakerController speakerController:
                    speakerController.IsOn = random.Next(0, 2) == 0;
                    break;
                case PowerController powerController:
                    // No-op
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
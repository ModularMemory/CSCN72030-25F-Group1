using System.Diagnostics;
using System.Text.Json;
using FalloutVault.Devices;
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

        using var controller = new DeviceController(_logger);

        controller.MessageBus.Handler += MessageBusOnMessage;
        AddDevices(controller);

        controller.Start();

        await ModifyDevices(controller);

        controller.Stop();
    }

    private static void MessageBusOnMessage(object? sender, DeviceMessage e)
    {
        _logger.Information("Device message: {@Message}", e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()
    }

    private static async Task ModifyDevices(DeviceController controller)
    {
        var random = new Random(69);
        var sw = Stopwatch.StartNew();

        const int RUN_SECONDS = 30;
        const int DEVICE_CHANGE_DELAY_MS = 250;
        while (sw.Elapsed.Seconds < RUN_SECONDS)
        {
            // Sleep
            await Task.Delay(TimeSpan.FromMilliseconds(DEVICE_CHANGE_DELAY_MS));

            // Get a random device
            var device = random.GetItems(controller.Devices.ToArray(), 1).First();

            // Do something to it
            switch (device)
            {
                case LightController lightController:
                    lightController.IsOn = random.Next(0, 3) == 0;
                    lightController.DimmerLevel = Math.Sqrt(random.NextDouble()); // sqrt to bias towards higher dimmer levels
                    break;
                case FanController fanController:
                    fanController.IsOn = random.Next(0, 3) == 0;
                    fanController.TargetRpm = random.Next(0, 2_000);
                    break;
                case PowerController powerController:
                    // No-op
                    break;
            }
        }
    }

    private static void AddDevices(DeviceController controller)
    {
        controller
            // Lights
            .AddDevice(new LightController(new DeviceId("Light-1", "East Hall"), (Watt)50))
            .AddDevice(new LightController(new DeviceId("Light-2", "East Hall"), (Watt)100))
            .AddDevice(new LightController(new DeviceId("Light-3", "East Hall"), (Watt)75))
            .AddDevice(new LightController(new DeviceId("Light-4", "West Hall"), (Watt)50))
            .AddDevice(new LightController(new DeviceId("Light-5", "North Hall"), (Watt)120))
            // Fans
            .AddDevice(new FanController(new DeviceId("Fan-1", "East Hall")))
            .AddDevice(new FanController(new DeviceId("Fan-3", "West Hall")))
            .AddDevice(new FanController(new DeviceId("Fan-2", "North Hall")))
            // Speaker controller
            .AddDevice(new SpeakerController(new DeviceId("Speaker-1", "East Hall"), (Watt)100))
            .AddDevice(new SpeakerController(new DeviceId("Speaker-2", "West Hall"), (Watt)100))
            .AddDevice(new SpeakerController(new DeviceId("Speaker-3", "North Hall"), (Watt)100))
            .AddDevice(new SpeakerController(new DeviceId("Speaker-4", "Generator Room"), (Watt)100))
            // Power Controller
            .AddDevice(new PowerController(new DeviceId("Central-Reactor", "Generator Room"), (Watt)1_000));
    }
}
using FalloutVault.Devices;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FalloutVault.ConsoleApp;

internal static class Program
{
    private static ILogger _logger = null!;

    public static async Task Main(string[] args)
    {
        // TODO: Replace with real logger impl
        _logger = new Logger<DeviceController>(new LoggerFactory());

        using var controller = new DeviceController(_logger);

        controller.MessageBus.Handler += MessageBusOnMessage;
        AddDevices(controller);

        controller.Start();

        await ModifyDevices(controller);

        controller.Stop();
    }

    private static void MessageBusOnMessage(object? sender, DeviceMessage e)
    {
        // TODO: Log it
    }

    private static async Task ModifyDevices(DeviceController controller)
    {
        var random = new Random(69);

        const int RUN_SECONDS = 30;
        const int DEVICE_CHANGE_DELAY_MS = 250;
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(RUN_SECONDS));
        await Task.Run(() =>
        {
            for (;;)
            {
                // Sleep
                Thread.Sleep(TimeSpan.FromMilliseconds(DEVICE_CHANGE_DELAY_MS));

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
        }, cts.Token);
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
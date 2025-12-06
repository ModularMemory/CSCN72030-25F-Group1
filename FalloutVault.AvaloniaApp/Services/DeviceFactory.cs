using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FalloutVault.AvaloniaApp.Services;

public static class DeviceFactory
{
    public static void AddDevices(IServiceProvider serviceProvider)
    {
        var deviceController = serviceProvider.GetRequiredService<IDeviceController>(); // I hate this, it should be a bus

        FanController coreFan;
        IDevice[] devices =
        [
            // Lights
            new LightController(new DeviceId("Light-1", "East Hall"), (Watt)50),
            new LightController(new DeviceId("Light-2", "East Hall"), (Watt)100),
            new LightController(new DeviceId("Light-4", "West Hall"), (Watt)50),
            new LightController(new DeviceId("Light-5", "North Hall"), (Watt)100),
            new LightController(new DeviceId("Light-6", "South Hall"), (Watt)100),
            new LightController(new DeviceId("Light-7", "Entrance"), (Watt)120),
            new LightController(new DeviceId("Grow-Light-1", "East Field"), (Watt)120),
            new LightController(new DeviceId("Grow-Light-2", "West Field"), (Watt)120),
            new LightController(new DeviceId("Light-3", "Generator Room"), (Watt)75),
            // Fans
            new FanController(new DeviceId("Fan-1", "East Hall"), (Watt)75, 2_000),
            new FanController(new DeviceId("Fan-3", "West Hall"), (Watt)50, 1_600),
            new FanController(new DeviceId("Fan-2", "North Hall"), (Watt)60, 1_800),
            new FanController(new DeviceId("Fan-4", "South Hall"), (Watt)100, 4_000),
            coreFan = new FanController(new DeviceId("Core-Fan", "Generator Room"), (Watt)200, 8_000),
            // Speaker controller
            new SpeakerController(new DeviceId("Speaker-1", "East Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-2", "West Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-3", "North Hall"), (Watt)100),
            new SpeakerController(new DeviceId("Speaker-4", "Generator Room"), (Watt)100),
            new SpeakerController(new DeviceId("Entrance-Speaker", "Entrance"), (Watt)100),
            // Doors
            new DoorController(new DeviceId("Door-1", "East Hall")),
            new DoorController(new DeviceId("Door-2", "East Hall")),
            new DoorController(new DeviceId("Door-3", "North Hall")),
            new DoorController(new DeviceId("Door-4", "West Hall")),
            new DoorController(new DeviceId("Door-5", "South Hall")),
            new DoorController(new DeviceId("Door-6", "Generator Room")),
            new DoorController(new DeviceId("Vault-Door", "Entrance")),
            // Vent seal controller
            new VentSealController(new DeviceId("VentSeal-1", "East Hall")),
            new VentSealController(new DeviceId("VentSeal-2", "West Hall")),
            new VentSealController(new DeviceId("VentSeal-3", "North Hall")),
            new VentSealController(new DeviceId("VentSeal-4", "South Hall")),
            new VentSealController(new DeviceId("VentSeal-5", "Generator Room")),
            // Crop sprinkler controller
            new CropSprinklerController(new DeviceId("CropSprinkler-1", "East Field"), (Watt)50),
            new CropSprinklerController(new DeviceId("CropSprinkler-2", "East Field"), (Watt)50),
            new CropSprinklerController(new DeviceId("CropSprinkler-3", "West Field"), (Watt)50),
            new CropSprinklerController(new DeviceId("CropSprinkler-4", "West Field"), (Watt)50),
            // Power Controller
            new PowerController(new DeviceId("Central-Reactor", "Generator Room"), (Watt)1_500, deviceController)
        ];

        InitializeDevices(devices, coreFan);

        var deviceRegistry = serviceProvider.GetRequiredService<IDeviceRegistry>();
        foreach (var device in devices)
        {
            deviceRegistry.RegisterDevice(device);
        }
    }

    private static void InitializeDevices(IDevice[] devices, FanController coreFan)
    {
        var sprinklerSections = Enum.GetValues<SprinklerSection>();

        var random = new Random(69);
        foreach (var device in devices)
        {
            if (device is IOnOff) device.SendCommand(new DeviceCommand.SetOn(random.Next(0, 3) > 0));
            if (device is IOpenClose) device.SendCommand(new DeviceCommand.SetOpen(random.Next(0, 2) > 0));
            if (device is ILockable) device.SendCommand(new DeviceCommand.SetLocked(random.Next(0, 2) > 0));
            if (device is IFanController) device.SendCommand(new DeviceCommand.SetFanTargetRpm(random.Next(0, 25) * 200));
            if (device is ICropSprinklerController) device.SendCommand(new DeviceCommand.SetCropSection(random.GetItems(sprinklerSections, 1)[0]));
            if (device is ILightController) device.SendCommand(new DeviceCommand.SetLightDimmer(Math.Sqrt(random.NextDouble()))); // sqrt to bias towards larger numbers
            if (device is ISpeakerController) device.SendCommand(new DeviceCommand.SetSpeakerVolume(Math.Sqrt(random.NextDouble()))); // sqrt to bias towards larger numbers
        }

        coreFan.SendCommand(new DeviceCommand.SetOn(true));
        coreFan.SendCommand(new DeviceCommand.SetFanTargetRpm(1_000));
    }
}
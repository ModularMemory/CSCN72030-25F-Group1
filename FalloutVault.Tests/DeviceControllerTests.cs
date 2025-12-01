using FalloutVault.Commands;
using FalloutVault.Devices.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;
using Serilog.Core;

namespace FalloutVault.Tests;

public class DeviceControllerTests
{
    private static void SetupPrerequisites(
        out DeviceRegistry registry,
        out MockDeviceMessageEventBus messageBus,
        out MockPowerEventBus powerBus,
        out DeviceController controller)
    {
        registry = new DeviceRegistry(Logger.None);
        messageBus = new MockDeviceMessageEventBus();
        powerBus = new MockPowerEventBus();
        controller = new DeviceController(registry, messageBus, powerBus, Logger.None);
    }

    [Test]
    public void DeviceController_SendCommand_SendsCommandToCorrectDevice()
    {
        // Arrange
        SetupPrerequisites(out var registry, out var messageBus, out var powerBus, out var controller);
        var devices = Enumerable.Range(TestUtils.LineNumber(), 20)
            .Select(x => new CommandableDevice(DeviceIdGenerator.GetRandomDeviceId(x), DeviceType.Unknown))
            .ToArray();
        foreach (var device in devices) registry.RegisterDevice(device);

        var targetDevice = devices[devices.Length / 2];

        // Act
        var setOn = new DeviceCommand.SetOn(true);
        controller.SendCommand(targetDevice.Id, setOn);

        // Assert
        Assert.That(targetDevice.Commands, Has.Count.EqualTo(1));
        Assert.That(targetDevice.Commands[0], Is.SameAs(setOn));
        foreach (var device in devices.Where(x => !ReferenceEquals(x, targetDevice)))
        {
            Assert.That(device.Commands, Is.Empty);
        }
    }

    [Test]
    public void DeviceController_SendZonedCommand_SendsCommandsToCorrectDevices()
    {
        // Arrange
        SetupPrerequisites(out var registry, out var messageBus, out var powerBus, out var controller);
        var devices = Enumerable.Range(TestUtils.LineNumber(), 20)
            .Select(x => new DeviceId(DeviceIdGenerator.GetRandomDeviceName(x), DeviceIdGenerator.GetRandomDeviceZone(x / 3)))
            .Select(x => new CommandableDevice(x, DeviceType.Unknown))
            .ToArray();
        foreach (var device in devices) registry.RegisterDevice(device);

        var targetZone = devices[devices.Length / 2].Id.Zone;
        var targetDevices = devices
            .Where(x => x.Id.Zone == targetZone)
            .ToArray();
        Assert.That(targetDevices, Has.Length.GreaterThan(1));

        // Act
        var setOn = new DeviceCommand.SetOn(true);
        controller.SendZonedCommand(targetZone, setOn);

        // Assert
        foreach (var device in targetDevices)
        {
            Assert.That(device.Commands, Has.Count.EqualTo(1));
            Assert.That(device.Commands[0], Is.SameAs(setOn));
        }
        foreach (var device in devices.Where(x => x.Id.Zone != targetZone))
        {
            Assert.That(device.Commands, Is.Empty);
        }
    }

    [Test]
    public void DeviceController_SendBroadcastCommand_SendsCommandsToAllDevices()
    {
        // Arrange
        SetupPrerequisites(out var registry, out var messageBus, out var powerBus, out var controller);
        var devices = Enumerable.Range(TestUtils.LineNumber(), 20)
            .Select(x => new CommandableDevice(DeviceIdGenerator.GetRandomDeviceId(x), DeviceType.Unknown))
            .ToArray();
        foreach (var device in devices) registry.RegisterDevice(device);

        // Act
        var setOn =  new DeviceCommand.SetOn(true);
        controller.SendBroadcastCommand(setOn);

        // Assert
        foreach (var device in devices)
        {
            Assert.That(device.Commands, Has.Count.EqualTo(1));
            Assert.That(device.Commands[0], Is.SameAs(setOn));
        }
    }
}
using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

public class VentSealTests
{
    [Test]
    public void VentSeal_TurnOn_PublishesStateChangeMessage()
    {
        // Arrange
        var ventSealController = new VentSealController(DeviceIdGenerator.GetRandomDeviceId());
        ventSealController.SendCommand(new DeviceCommand.SetVentOpen(false));

        var eventBus = new MockDeviceMessageEventBus();
        ventSealController.SetEventBus(eventBus);

        // Act
        ventSealController.SendCommand(new DeviceCommand.SetVentOpen(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.VentStateChanged>());
    }

    [Test]
    public void VentSeal_TurnOff_PublishesStateChangeMessage()
    {
        // Arrange
        var ventSealController = new VentSealController(DeviceIdGenerator.GetRandomDeviceId());
        ventSealController.SendCommand(new DeviceCommand.SetVentOpen(true));

        var eventBus = new MockDeviceMessageEventBus();
        ventSealController.SetEventBus(eventBus);

        // Act
        ventSealController.SendCommand(new DeviceCommand.SetVentOpen(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.VentStateChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }
    [Test]
    public void VentSeal_LockStateChange_PublishesLockStateChangedMessage()
    {
        // Arrange
        var ventSealController = new VentSealController(DeviceIdGenerator.GetRandomDeviceId());
        ventSealController.SendCommand(new DeviceCommand.SetVentLocked(false));

        var eventBus = new MockDeviceMessageEventBus();
        ventSealController.SetEventBus(eventBus);

        // Act
        ventSealController.SendCommand(new DeviceCommand.SetVentLocked(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.VentLockedChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }
}
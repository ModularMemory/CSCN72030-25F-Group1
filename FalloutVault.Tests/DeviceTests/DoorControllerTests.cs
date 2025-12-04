using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

public class DoorControllerTests
{
    [Test]
    public void DoorController_TurnOn_PublishesStateChangeMessage()
    {
        // Arrange
        var DoorController = new DoorController(DeviceIdGenerator.GetRandomDeviceId());
        DoorController.SendCommand(new DeviceCommand.SetOpen(false));

        var eventBus = new MockDeviceMessageEventBus();
        DoorController.SetEventBus(eventBus);

        // Act
        DoorController.SendCommand(new DeviceCommand.SetOpen(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.DoorOpenCloseChanged>());
    }

    [Test]
    public void DoorController_TurnOff_PublishesStateChangeMessage()
    {
        // Arrange
        var DoorController = new DoorController(DeviceIdGenerator.GetRandomDeviceId());
        DoorController.SendCommand(new DeviceCommand.SetOpen(true));

        var eventBus = new MockDeviceMessageEventBus();
        DoorController.SetEventBus(eventBus);

        // Act
        DoorController.SendCommand(new DeviceCommand.SetOpen(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.DoorOpenCloseChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }
    [Test]
    public void DoorController_LockStateChange_PublishesLockChangedMessage()
    {
        // Arrange
        var DoorController = new DoorController(DeviceIdGenerator.GetRandomDeviceId());
        DoorController.SendCommand(new DeviceCommand.SetDoorLocked(true));

        var eventBus = new MockDeviceMessageEventBus();
        DoorController.SetEventBus(eventBus);

        // Act
        DoorController.SendCommand(new DeviceCommand.SetDoorLocked(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.DoorLockChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }
}
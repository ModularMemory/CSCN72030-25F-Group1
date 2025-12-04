using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

internal class CropSprinklerControllerTests
{
    [Test]
    public void CropSprinkler_TurnOn_PublishesStateChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)0);
        cropSprinklerController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);

    }

    [Test]
    public void CropSprinkler_TurnOff_PublishesStateChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)0);
        cropSprinklerController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void CropSprinkler_SectionChange_PublishesSectionChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)0);
        cropSprinklerController.SendCommand(new DeviceCommand.CurrentCropSection(1));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.CurrentCropSection(2));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerSectionChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(2));
    }
    }
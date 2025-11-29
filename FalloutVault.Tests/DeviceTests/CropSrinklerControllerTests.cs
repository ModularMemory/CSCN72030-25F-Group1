using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutVault.Tests.DeviceTests;
internal class CropSrinklerControllerTests
{
    [Test]
    public void CropSprinkler_TurnOn_PublishesStateChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId());
        cropSprinklerController.SendCommand(new DeviceCommand.IsOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.IsOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerStateChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);

    }

    [Test]
    public void CropSprinkler_TurnOff_PublishesStateChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId());
        cropSprinklerController.SendCommand(new DeviceCommand.IsOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.IsOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerStateChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void CropSprinkler_SectionChange_PublishesSectionAmountChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId());
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

    [Test]
    public void CropSprinkler_NoSectionChange_PublishesSectionAmountDoesNotChangeMessage()
    {
        // Arrange
        var cropSprinklerController = new CropSprinklerController(DeviceIdGenerator.GetRandomDeviceId());
        cropSprinklerController.SendCommand(new DeviceCommand.CurrentCropSection(1));

        var eventBus = new MockDeviceMessageEventBus();
        cropSprinklerController.SetEventBus(eventBus);

        // Act
        cropSprinklerController.SendCommand(new DeviceCommand.CurrentCropSection(1));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.CropSprinklerSectionChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(1));
    }
}
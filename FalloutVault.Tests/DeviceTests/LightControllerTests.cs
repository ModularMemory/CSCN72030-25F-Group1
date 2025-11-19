using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

public class LightControllerTests
{
    [Test]
    public void LightController_TurnOn_PublishesLightOnMessage()
    {
        // Arrange
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25)
        {
            IsOn = false,
        };

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnChanged>());
    }

    [Test]
    public void LightController_TurnOff_PublishesLightOffMessage()
    {
        // Arrange
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25)
        {
            IsOn = true,
        };

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void LightController_SetDimmer_PublishesDimmerChangedMessage()
    {
        // Arrange
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25)
        {
            DimmerLevel = 1.0,
        };

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetLightDimmer(0.5));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.DimmerLevelChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(0.5).Within(0.01));
    }

    [Test]
    public void LightController_TurnOnFor_TurnsDeviceOnForSpecifiedTime()
    {
        // Arrange
        var onTime = TimeSpan.FromMilliseconds(10);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25)
        {
            IsOn = false,
        };

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(lightController.IsOn, Is.False);

        lightController.SendCommand(new DeviceCommand.TurnOnFor(onTime));
        Assert.That(lightController.IsOn, Is.True);

        Thread.Sleep(onTime);
        lightController.Update();
        Assert.That(lightController.IsOn, Is.False);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.LightOnChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.False);
    }

    [Test]
    public void LightController_TurnOffFor_TurnsDeviceOffForSpecifiedTime()
    {
        // Arrange
        var offTime = TimeSpan.FromMilliseconds(10);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25)
        {
            IsOn = true,
        };

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(lightController.IsOn, Is.True);

        lightController.SendCommand(new DeviceCommand.TurnOffFor(offTime));
        Assert.That(lightController.IsOn, Is.False);

        Thread.Sleep(offTime);
        lightController.Update();
        Assert.That(lightController.IsOn, Is.True);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.LightOnChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.True);
    }
}
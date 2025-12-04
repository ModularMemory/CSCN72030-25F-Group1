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
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        lightController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
    }

    [Test]
    public void LightController_TurnOff_PublishesLightOffMessage()
    {
        // Arrange
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        lightController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void LightController_SetDimmer_PublishesDimmerChangedMessage()
    {
        // Arrange
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        lightController.SendCommand(new DeviceCommand.SetLightDimmer(1.0));

        var eventBus = new MockDeviceMessageEventBus();
        lightController.SetEventBus(eventBus);

        // Act
        const double EXPECTED = 0.5;
        lightController.SendCommand(new DeviceCommand.SetLightDimmer(EXPECTED));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightDimmerLevelChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(EXPECTED).Within(0.01));
    }

    [Test]
    public void LightController_TurnOnFor_TurnsDeviceOnForSpecifiedTime()
    {
        // Arrange
        var onTime = TimeSpan.FromMilliseconds(10);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        lightController.SendCommand(new DeviceCommand.SetOn(false));

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
        Assert.That(eventBus.Messages, Has.Count.EqualTo(3));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.LightTimedOnOffChanged>());
        Assert.That(eventBus.Messages[2], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[2].Data, Is.False);
    }

    [Test]
    public void LightController_TurnOffFor_TurnsDeviceOffForSpecifiedTime()
    {
        // Arrange
        var offTime = TimeSpan.FromMilliseconds(10);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        lightController.SendCommand(new DeviceCommand.SetOn(true));

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
        Assert.That(eventBus.Messages, Has.Count.EqualTo(3));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.LightTimedOnOffChanged>());
        Assert.That(eventBus.Messages[2], Is.TypeOf<DeviceMessage.LightOnOffChanged>());
        Assert.That(eventBus.Messages[2].Data, Is.True);
    }
}
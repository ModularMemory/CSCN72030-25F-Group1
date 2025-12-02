using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;
public class PowerControllerTests
{
    [Test]
    public void PowerController_ReceivePowerDraw_PublishesTotalPowerDrawChangedMessage()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)100);

        var powerEventBus = new PowerEventBus();
        powerController.SetEventBus(powerEventBus);
        lightController.SetEventBus(powerEventBus);

        var eventBus = new MockDeviceMessageEventBus();
        powerController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.TotalPowerDrawChanged>());
    }

    [Test]
    public void PowerController_UsageExceeded_ShutsDownDevice()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)150);
        var light1 = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)100);
        var light2 = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)100);

        var powerEventBus = new PowerEventBus();
        powerController.SetEventBus(powerEventBus);
        light1.SetEventBus(powerEventBus);
        light2.SetEventBus(powerEventBus);

        light1.SendCommand(new DeviceCommand.SetOn(true));

        // Act
        light2.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(light2.IsOn, Is.False);
    }

    [Test]
    public void PowerController_Efficiency_CalculatesCorrectly()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);

        // Act & Assert
        Assert.That(powerController.Efficiency, Is.EqualTo(1.0).Within(0.01));
    }

    [Test]
    public void PowerController_TurnedOff_SetsGenerationToZero()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);

        // Act
        powerController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(powerController.PowerGeneration.W, Is.EqualTo(0));
        Assert.That(powerController.IsOn, Is.False);
    }

    [Test]
    public void PowerController_TurnedOn_RestoresPowerGeneration()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        powerController.SendCommand(new DeviceCommand.SetOn(false));

        // Act
        powerController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(powerController.PowerGeneration.W, Is.EqualTo(1000));
        Assert.That(powerController.IsOn, Is.True);
    }

    [Test]
    public void PowerController_Shutdown_PublishesPowerChangedMessage()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);

        var eventBus = new MockDeviceMessageEventBus();
        powerController.SetEventBus(eventBus);

        // Act
        powerController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.GreaterThan(0));
        var shutdownMessage = eventBus.Messages
            .OfType<DeviceMessage.PowerOnOffChanged>()
            .FirstOrDefault();
        Assert.That(shutdownMessage, Is.Not.Null);
    }

    [Test]
    public void PowerController_OnTurnOff_TurnsOffDevices()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);

        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        var powerEventBus = new PowerEventBus();
        var eventBus = new MockDeviceMessageEventBus();

        powerController.SetEventBus(powerEventBus);
        lightController.SetEventBus(eventBus);

        // Act
        lightController.SendCommand(new DeviceCommand.SetOn(true));

        powerController.SendCommand(new DeviceCommand.SetOn(false));


        // Assert
        Assert.That(lightController.IsOn, Is.False);
    }
}
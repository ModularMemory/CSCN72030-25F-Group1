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
    public void PowerController_RequestPower_ApprovesWhenPowerAvailable()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(1000));
        var deviceId = DeviceIdGenerator.GetRandomDeviceId();

        // Act
        var result = powerController.RequestPower(deviceId, new Watt(500));

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void PowerController_RequestPower_DeniesWhenPowerNotAvailable()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(1000));
        var device1 = DeviceIdGenerator.GetRandomDeviceId();
        var device2 = DeviceIdGenerator.GetRandomDeviceId();

        // Act
        powerController.RequestPower(device1, new Watt(800));
        var result = powerController.RequestPower(device2, new Watt(300));

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void PowerController_ReleasePower_FreesAllocatedPower()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(1000));
        var device1 = DeviceIdGenerator.GetRandomDeviceId();
        var device2 = DeviceIdGenerator.GetRandomDeviceId();

        powerController.RequestPower(device1, new Watt(800));

        // Act
        powerController.ReleasePower(device1);
        var result = powerController.RequestPower(device2, new Watt(800));

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void PowerController_ReceivePowerDraw_PublishesTotalPowerDrawChangedMessage()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(1000));
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(100));

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
}

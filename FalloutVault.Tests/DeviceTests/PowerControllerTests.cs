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
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        var deviceId = DeviceIdGenerator.GetRandomDeviceId();

        powerController.SendCommand(new DeviceCommand.RequestPower(deviceId, (Watt)500));

        Assert.That(powerController.LastRequestResult, Is.True);
    }

    [Test]
    public void PowerController_RequestPower_DeniesWhenPowerNotAvailable()
    {
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        var device1 = DeviceIdGenerator.GetRandomDeviceId();
        var device2 = DeviceIdGenerator.GetRandomDeviceId();

        powerController.SendCommand(new DeviceCommand.RequestPower(device1, (Watt)800));
        powerController.SendCommand(new DeviceCommand.RequestPower(device2, (Watt)300));

        Assert.That(powerController.LastRequestResult, Is.False);
    }

    [Test]
    public void PowerController_ReleasePower_FreesAllocatedPower()
    {
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        var device1 = DeviceIdGenerator.GetRandomDeviceId();
        var device2 = DeviceIdGenerator.GetRandomDeviceId();

        powerController.SendCommand(new DeviceCommand.RequestPower(device1, (Watt)800));

        powerController.SendCommand(new DeviceCommand.ReleasePower(device1));
        powerController.SendCommand(new DeviceCommand.RequestPower(device2, (Watt)800));

        Assert.That(powerController.LastRequestResult, Is.True);
    }

    [Test]
    public void PowerController_ReceivePowerDraw_PublishesTotalPowerDrawChangedMessage()
    {
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)1000);
        var lightController = new LightController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)100);

        var powerEventBus = new PowerEventBus();
        powerController.SetEventBus(powerEventBus);
        lightController.SetEventBus(powerEventBus);

        var eventBus = new MockDeviceMessageEventBus();
        powerController.SetEventBus(eventBus);

        lightController.SendCommand(new DeviceCommand.SetOn(true));

        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.TotalPowerDrawChanged>());
    }

    [Test]
    public void PowerController_UsageExceeded_ShutsDownDevice()
    {
        // Arrange
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(150));
        var light1 = new LightController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(100));
        var light2 = new LightController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(100));


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
        var powerController = new PowerController(DeviceIdGenerator.GetRandomDeviceId(), new Watt(1000));

        // Act & Assert
        Assert.That(powerController.Efficiency, Is.EqualTo(1.0).Within(0.01));
    }
}

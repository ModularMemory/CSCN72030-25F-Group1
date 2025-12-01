using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

public class FanControllerTests
{
    [Test]
    public void FanController_TurnOn_PublishesFanOnMessage()
    {
        // Arrange
        var fanController = new FanController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25, 1_000);
        fanController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        fanController.SetEventBus(eventBus);

        // Act
        fanController.SendCommand(new DeviceCommand.SetOn(true));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
    }

    [Test]
    public void FanController_TurnOff_PublishesFanOffMessage()
    {
        // Arrange
        var fanController = new FanController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25, 1_000);
        fanController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        fanController.SetEventBus(eventBus);

        // Act
        fanController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void FanController_SetTargetRpm_PublishesTargetRpmChangedMessage()
    {
        // Arrange
        const int EXPECTED_RPM = 1_000;
        var fanController = new FanController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25, EXPECTED_RPM);
        fanController.SendCommand(new DeviceCommand.SetFanTargetRpm(0));

        var eventBus = new MockDeviceMessageEventBus();
        fanController.SetEventBus(eventBus);

        // Act
        fanController.SendCommand(new DeviceCommand.SetFanTargetRpm(EXPECTED_RPM));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.FanTargetRpmChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(EXPECTED_RPM));
    }

    [Test]
    public void FanController_TurnOnFor_TurnsDeviceOnForSpecifiedTime()
    {
        // Arrange
        var onTime = TimeSpan.FromMilliseconds(10);
        var fanController = new FanController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25, 1_000);
        fanController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        fanController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(fanController.IsOn, Is.False);

        fanController.SendCommand(new DeviceCommand.TurnOnFor(onTime));
        Assert.That(fanController.IsOn, Is.True);

        Thread.Sleep(onTime);
        fanController.Update();
        Assert.That(fanController.IsOn, Is.False);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.False);
    }

    [Test]
    public void FanController_TurnOffFor_TurnsDeviceOffForSpecifiedTime()
    {
        // Arrange
        var offTime = TimeSpan.FromMilliseconds(10);
        var fanController = new FanController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25, 1_000);
        fanController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        fanController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(fanController.IsOn, Is.True);

        fanController.SendCommand(new DeviceCommand.TurnOffFor(offTime));
        Assert.That(fanController.IsOn, Is.False);

        Thread.Sleep(offTime);
        fanController.Update();
        Assert.That(fanController.IsOn, Is.True);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.FanOnOffChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.True);
    }
}
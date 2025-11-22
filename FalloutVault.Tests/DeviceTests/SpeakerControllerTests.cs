using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Tests.Mocks;
using FalloutVault.Tests.Utils;

namespace FalloutVault.Tests.DeviceTests;

public class SpeakerControllerTests
{
    [Test]
    public void SpeakerController_TurnOn_PublishesSpeakerOnMessage()
    {
        //Arrange
        var speakerController = new SpeakerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        speakerController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        speakerController.SetEventBus(eventBus);

        //Act
        speakerController.SendCommand(new DeviceCommand.SetOn(true));

        //Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
    }

    [Test]
    public void SpeakerController_TurnOff_PublishesSpeakerOffMessage()
    {
        // Arrange
        var speakerController = new SpeakerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        speakerController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        speakerController.SetEventBus(eventBus);

        // Act
        speakerController.SendCommand(new DeviceCommand.SetOn(false));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
    }

    [Test]
    public void SpeakerController_SetVolume_PublishesVolumeChangedMessage()
    {
        // Arrange
        var speakerController = new SpeakerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        speakerController.SendCommand(new DeviceCommand.SetSpeakerVolume(1.0));

        var eventBus = new MockDeviceMessageEventBus();
        speakerController.SetEventBus(eventBus);

        // Act
        speakerController.SendCommand(new DeviceCommand.SetSpeakerVolume(0.5));

        // Assert
        Assert.That(eventBus.Messages, Has.Count.EqualTo(1));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.VolumeLevelChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.EqualTo(0.5).Within(0.01));
    }

    [Test]
    public void SpeakerController_TurnOnFor_TurnsDeviceOnForSpecifiedTime()
    {
        // Arrange
        var onTime = TimeSpan.FromMilliseconds(10);
        var speakerController = new SpeakerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        speakerController.SendCommand(new DeviceCommand.SetOn(false));

        var eventBus = new MockDeviceMessageEventBus();
        speakerController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(speakerController.IsOn, Is.False);

        speakerController.SendCommand(new DeviceCommand.TurnOnFor(onTime));
        Assert.That(speakerController.IsOn, Is.True);

        Thread.Sleep(onTime);
        speakerController.Update();
        Assert.That(speakerController.IsOn, Is.False);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.True);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.False);
    }

    [Test]
    public void SpeakerController_TurnOffFor_TurnsDeviceOffForSpecifiedTime()
    {
        // Arrange
        var offTime = TimeSpan.FromMilliseconds(10);
        var speakerController = new SpeakerController(DeviceIdGenerator.GetRandomDeviceId(), (Watt)25);
        speakerController.SendCommand(new DeviceCommand.SetOn(true));

        var eventBus = new MockDeviceMessageEventBus();
        speakerController.SetEventBus(eventBus);

        // Act + Assert on-ness
        Assert.That(speakerController.IsOn, Is.True);

        speakerController.SendCommand(new DeviceCommand.TurnOffFor(offTime));
        Assert.That(speakerController.IsOn, Is.False);

        Thread.Sleep(offTime);
        speakerController.Update();
        Assert.That(speakerController.IsOn, Is.True);

        // Assert event bus
        Assert.That(eventBus.Messages, Has.Count.EqualTo(2));
        Assert.That(eventBus.Messages[0], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
        Assert.That(eventBus.Messages[0].Data, Is.False);
        Assert.That(eventBus.Messages[1], Is.TypeOf<DeviceMessage.SpeakerOnChanged>());
        Assert.That(eventBus.Messages[1].Data, Is.True);
    }
}

using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class SpeakerController : PoweredDevice, ISpeakerController
{
    // Fields
    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();

    // Properties

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.SpeakerController;
    public Watt SpeakerWattage { get; }

    public bool IsOn
    {
        get;
        set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.SpeakerOnOffChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    public double Volume
    {
        get;
        set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.VolumeLevelChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors

    public SpeakerController(DeviceId id, Watt speakerWattage)
    {
        Id = id;
        SpeakerWattage = speakerWattage;
        Volume = 1;
    }

    // Methods

    public override void Update()
    {
        lock (_timerLock)
        {
            if (_deviceTimer.CheckCompleted())
            {
                IsOn = _deviceTimer.State;
            }
        }
    }

    public override void SendCommand(DeviceCommand command)
    {
        switch(command)
        {
            case DeviceCommand.SetOn setOn:
                lock (_timerLock)
                {
                    _deviceTimer.Cancel();
                }

                IsOn = setOn.IsOn;
                break;
            case DeviceCommand.TurnOnFor turnOnFor:
                TurnOnFor(turnOnFor.Time);
                break;
            case DeviceCommand.TurnOffFor turnOffFor:
                TurnOffFor(turnOffFor.Time);
                break;
            case DeviceCommand.SetSpeakerVolume setSpeakerVolume:
                Volume = setSpeakerVolume.Volume;
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.SpeakerOnOffChanged(IsOn));
                PublishMessage(new DeviceMessage.VolumeLevelChanged(Volume));
                PublishMessage(new DeviceMessage.SpeakerWattage(SpeakerWattage));
                break;
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
        {
            return Watt.Zero;
        }

        return SpeakerWattage * Volume;
    }

    public void TurnOnFor(TimeSpan time)
    {
        lock (_timerLock)
        {
            _deviceTimer.SetTimer(time, false);
            IsOn = true;
        }
    }

    public void TurnOffFor(TimeSpan time)
    {
        lock (_timerLock)
        {
            _deviceTimer.SetTimer(time, true);
            IsOn = false;
        }
    }
}
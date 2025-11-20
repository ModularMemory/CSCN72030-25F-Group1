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

            PublishMessage(new DeviceMessage.SpeakerOnChanged(field));

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
        Volume = 50;
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
            case DeviceCommand.SetOn:
                IsOn = (bool)command.Data!;
                break;
            case DeviceCommand.TurnOnFor:
                TurnOnFor((TimeSpan)command.Data!);
                break;
            case DeviceCommand.TurnOffFor:
                TurnOffFor((TimeSpan)command.Data!);
                break;
            case DeviceCommand.SetSpeakerVolume:
                Volume = (double)command.Data!;
                break;
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
        {
            return Watt.Zero;
        }

        return SpeakerWattage * (Volume / 10);
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
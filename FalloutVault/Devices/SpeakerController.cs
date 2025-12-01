using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Serilog;

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

            //PublishMessage(new DeviceMessage.SpeakerOnOffChanged(field));

            string SpeakerOnMessage = new DeviceMessage.SpeakerOnOffChanged(field).Message;
            Log.Information(SpeakerOnMessage);

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
            case DeviceCommand.SetOn:
                lock (_timerLock)
                {
                    _deviceTimer.Cancel();
                }

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
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.SpeakerOnChanged(IsOn));
                PublishMessage(new DeviceMessage.VolumeLevelChanged(Volume));
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
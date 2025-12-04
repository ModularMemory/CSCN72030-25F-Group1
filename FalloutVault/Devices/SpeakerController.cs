using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Serilog;
using System.Runtime.CompilerServices;
using FalloutVault.Interfaces;

namespace FalloutVault.Devices;

public class SpeakerController : PoweredDevice, ISpeakerController
{
    // Fields
    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();
    private readonly Lock _drawsLock = new();
    private readonly IDeviceController? _deviceController;
    private readonly Dictionary<DeviceId, Watt> _devicePowerDraws = new();
    private Watt _totalPowerDraw;

    // Properties

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.SpeakerController;
    public Watt SpeakerWattage { get; }

    public Watt PowerGeneration
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;
            PublishMessage(new DeviceMessage.PowerGenerationChanged(field));
        }
    }

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

    private void PowerMessageReceived(object? sender, Watt totalPowerDraw)
    {
        if (sender is not IDevice device) return;

        lock (_drawsLock)
        {
            _devicePowerDraws[device.Id] = totalPowerDraw;
            _totalPowerDraw = new Watt(_devicePowerDraws.Values.Sum(x => x.W));
        }

        PublishMessage(new DeviceMessage.TotalPowerDrawChanged(
            new PowerDraw(_totalPowerDraw, SpeakerWattage)
        ));

        if (_totalPowerDraw.W > PowerGeneration.W && sender is IDevice triggeringDevice)
        {
            try
            {
                triggeringDevice.SendCommand(new DeviceCommand.SetOn(false));
            }
            catch
            {

            }
        }
    }

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
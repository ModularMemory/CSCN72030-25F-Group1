using System.Diagnostics;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public class SpeakerController : Device, ISpeakerController
{
    // Fields
    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();
    private readonly Watt _speakerWattage;

    private bool _activated;

    // Properties

   public override DeviceId Id { get; }
    public bool Activated
    {
        get => _activated;
        set
        {
            if(!SetField(ref _activated, value)) return;

            PublishMessage(_activated
                ? new DeviceMessage("Speaker turned on", ValueBoxes.True)
                : new DeviceMessage("Speaker turned off", ValueBoxes.False)
             );

            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors

    public SpeakerController(DeviceId id, Watt speakerWattage)
    {
        Id = id;
        _speakerWattage = speakerWattage;
    }

    // Methods

    public override void Update()
    {
        Debug.Assert(PowerDraw == ComputePowerDraw());

        lock (_timerLock)
        {
            if (_deviceTimer.IsRunning)
            {
                _deviceTimer.Update();

                if (!_deviceTimer.IsRunning)
                {
                    Activated = _deviceTimer.State;
                }
            }
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!Activated)
        {
            return Watt.Zero;
        }

        return _speakerWattage;
    }
    public void TurnOnFor(TimeSpan time)
    {
        lock (_timerLock)
        {
            _deviceTimer.SetTimer(time, false);
            Activated = true;
        }
    }

    public void TurnOffFor(TimeSpan time)
    {
        lock ( _timerLock)
        {
            _deviceTimer.SetTimer(time, true);
            Activated = false;
        }
    }
}
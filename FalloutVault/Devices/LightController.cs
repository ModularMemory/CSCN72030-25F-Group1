using System.Diagnostics;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class LightController : Device, ILightController
{
    // Fields
    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();
    private readonly Watt _bulbWattage;

    private bool _isOn;
    private double _dimmerLevel;

    // Properties

    public override DeviceId Id { get; }

    public bool IsOn
    {
        get => _isOn;
        set
        {
            if (!SetField(ref _isOn, value)) return;

            PublishMessage(_isOn
                ? new DeviceMessage("Light turned on", ValueBoxes.True)
                : new DeviceMessage("Light turned off", ValueBoxes.False)
            );

            PowerDraw = ComputePowerDraw();
        }
    }

    public double DimmerLevel
    {
        get => _dimmerLevel;
        set
        {
            if (!SetField(ref _dimmerLevel, value)) return;

            PublishMessage(new DeviceMessage("Light dimmer level changed", _dimmerLevel));
            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors

    public LightController(DeviceId id, Watt bulbWattage)
    {
        Id = id;
        _bulbWattage = bulbWattage;
        DimmerLevel = 1;
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
                    IsOn = _deviceTimer.State;
                }
            }
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;

        return _bulbWattage * DimmerLevel;
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
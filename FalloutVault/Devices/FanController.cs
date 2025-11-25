using System.Diagnostics;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using FalloutVault.Utils;

namespace FalloutVault.Devices;

public class FanController : PoweredDevice, IFanController
{
    // Fields

    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();
    private readonly Watt _motorWattage;
    private readonly double _maxRpm;
    private long _lastSpeedUpdate;

    // Properties

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.FanController;

    public bool IsOn
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.FanOnChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    public double TargetRpm
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.FanTargetRpmChanged(field));
        }
    }

    public double SpeedRpm
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.FanSpeedRpmChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors
    public FanController(DeviceId id, Watt motorWattage, double maxRpm)
    {
        _motorWattage = motorWattage;
        _maxRpm = maxRpm;
        Id = id;
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

        var currentTimestamp = Stopwatch.GetTimestamp();
        var delta = (currentTimestamp - _lastSpeedUpdate) / (double)Stopwatch.Frequency;

        var targetSpeed = IsOn ? TargetRpm : 0;
        SpeedRpm = double.Lerp(targetSpeed, SpeedRpm, delta);
        _lastSpeedUpdate = currentTimestamp;
    }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case DeviceCommand.SetOn setOn:
                // Cancel the timer if the light was turned on/off manually
                lock (_timerLock)
                    _deviceTimer.Cancel();

                IsOn = setOn.IsOn;
                break;
            case DeviceCommand.SetFanTargetRpm setFanTargetRpm:
                TargetRpm = setFanTargetRpm.TargetRpm;
                break;
            case DeviceCommand.TurnOnFor turnOnFor:
                TurnOnFor(turnOnFor.Time);
                break;
            case DeviceCommand.TurnOffFor turnOffFor:
                TurnOffFor(turnOffFor.Time);
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.FanOnChanged(IsOn));
                PublishMessage(new DeviceMessage.FanTargetRpmChanged(TargetRpm));
                break;
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;

        return (Watt)MathUtils.Remap(SpeedRpm, 0, _maxRpm, 0, _motorWattage.W);
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
            _deviceTimer.SetTimer(time, false);
            IsOn = true;
        }
    }
}
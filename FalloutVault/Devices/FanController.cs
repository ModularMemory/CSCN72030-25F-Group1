using System.ComponentModel.DataAnnotations;
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
    private readonly Random _rpmStepRandom = new();
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

            PublishMessage(new DeviceMessage.FanOnOffChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    public Watt MotorWattage { get; }

    public int MaxRpm { get; }

    public int TargetRpm
    {
        get;
        private set
        {
            if (!SetField(ref field, Math.Min(value, MaxRpm))) return;

            PublishMessage(new DeviceMessage.FanTargetRpmChanged(field));
        }
    }

    public double SpeedRpm
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.FanSpeedRpmChanged((int)field));

            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors
    public FanController(DeviceId id, Watt motorWattage, [Range(0, int.MaxValue)] int maxRpm)
    {
        MotorWattage = motorWattage;
        MaxRpm = maxRpm;
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
        var deltaL = currentTimestamp - _lastSpeedUpdate;
        var delta = deltaL / (double)Stopwatch.Frequency;

        var targetSpeed = IsOn && TargetRpm >= 50
            ? TargetRpm + _rpmStepRandom.Next(-15, 15)
            : 0;

        var step = _rpmStepRandom.Next(35, 65) * delta * 3;

        SpeedRpm = Math.Max(
            0,
            SpeedRpm + (SpeedRpm < targetSpeed ? step : -step)
        );

        _lastSpeedUpdate += deltaL;
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
                PublishMessage(new DeviceMessage.FanMotorWattage(MotorWattage));
                PublishMessage(new DeviceMessage.FanMaxRpm(MaxRpm));
                PublishMessage(new DeviceMessage.FanOnOffChanged(IsOn));
                PublishMessage(new DeviceMessage.FanTargetRpmChanged(TargetRpm));
                break;
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;

        return (Watt)MathUtils.Remap(SpeedRpm, 0, MaxRpm, 0, MotorWattage.W);
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
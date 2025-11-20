using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class LightController : PoweredDevice, ILightController
{
    // Fields
    private readonly DeviceTimer<bool> _deviceTimer = new();
    private readonly Lock _timerLock = new();

    // Properties

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.LightController;
    public Watt BulbWattage { get; }

    public bool IsOn
    {
        get;
        set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.LightOnChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    public double DimmerLevel
    {
        get;
        set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.DimmerLevelChanged(field));

            PowerDraw = ComputePowerDraw();
        }
    }

    // Constructors

    public LightController(DeviceId id, Watt bulbWattage)
    {
        Id = id;
        BulbWattage = bulbWattage;
        DimmerLevel = 1;
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
        switch (command)
        {
            case DeviceCommand.SetOn:
                IsOn = (bool)command.Data!;
                break;
            case DeviceCommand.SetLightDimmer:
                DimmerLevel = (double)command.Data!;
                break;
            case DeviceCommand.TurnOnFor:
                TurnOnFor((TimeSpan)command.Data!);
                break;
            case DeviceCommand.TurnOffFor:
                TurnOffFor((TimeSpan)command.Data!);
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.LightOnChanged(IsOn));
                PublishMessage(new DeviceMessage.DimmerLevelChanged(DimmerLevel));
                break;
        }
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;

        return BulbWattage * DimmerLevel;
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
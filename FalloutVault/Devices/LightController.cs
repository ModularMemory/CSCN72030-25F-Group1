using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public class LightController : Device, ILightController
{
    // Fields
    private readonly WattHours _bulbWattage;

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

            PowerDraw = PowerDraw = ComputePowerDraw();
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

    public LightController(DeviceId id, WattHours bulbWattage)
    {
        Id = id;
        _bulbWattage = bulbWattage;
    }

    // Methods

    public override void Update()
    {
        throw new NotImplementedException();
    }

    protected override WattHours ComputePowerDraw()
    {
        if (!IsOn)
            return WattHours.Zero;

        return _bulbWattage * DimmerLevel;
    }
}
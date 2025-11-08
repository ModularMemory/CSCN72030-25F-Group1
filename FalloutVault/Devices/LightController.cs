using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public class LightController : Device, ILightController
{
    // Fields

    private bool _isOn;
    private double _dimmerLevel;

    // Properties

    public override DeviceId Id { get; }

    public bool IsOn
    {
        get => _isOn;
        set
        {
            if (SetField(ref _isOn, value))
            {
                PublishMessage(
                    _isOn
                        ? new DeviceMessage("Light turned on", ValueBoxes.True)
                        : new DeviceMessage("Light turned off", ValueBoxes.False)
                );
            }
        }
    }

    public double DimmerLevel
    {
        get => _dimmerLevel;
        set
        {
            if (SetField(ref _dimmerLevel, value))
            {
                PublishMessage(new DeviceMessage("Light dimmer level changed", _dimmerLevel));
            }
        }
    }

    // Constructors

    public LightController(DeviceId id)
    {
        Id = id;
    }

    // Methods

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public void TurnOnFor(TimeSpan time)
    {
        throw new NotImplementedException();
    }

    public void TurnOffFor(TimeSpan time)
    {
        throw new NotImplementedException();
    }
}
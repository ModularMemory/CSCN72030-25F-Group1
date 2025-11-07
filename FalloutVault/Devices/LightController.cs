using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;

namespace FalloutVault.Devices;

public class LightController : Device, ILightController
{
    // Fields

    private bool _isOn;
    private double _dimmerLevel;

    // Properties

    public override DeviceId Id { get; }

    // TODO: Notification on setters
    public bool IsOn
    {
        get => _isOn;
        set => _isOn = value;
    }

    public double DimmerLevel
    {
        get => _dimmerLevel;
        set => _dimmerLevel = value;
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
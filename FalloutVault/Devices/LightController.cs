using FalloutVault.Devices.Interfaces;

namespace FalloutVault.Devices;

public class LightController : Device, ILightController
{
    // Fields

    private bool _isOn;
    private double _dimmerLevel;

    // Properties

    public override string Name { get; }
    public override string Zone { get; }

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

    public LightController(string name, string zone)
    {
        Name = name;
        Zone = zone;
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
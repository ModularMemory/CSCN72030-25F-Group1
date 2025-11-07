using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;

namespace FalloutVault.Devices;

public class FanController : Device, IFanController
{
    //fields
    private bool _IsOn;

    private double _TargetRpm;

    private double _SpeedRpm;

    //properties

    public override DeviceId Id { get; }

    //TODO: Notification on setters
    public bool IsOn
    {
        get => _IsOn;
        set => _IsOn = value;
    }

    public double TargetRpm
    {
        get => _TargetRpm;
        set => _TargetRpm = value;
    }

    public double SpeedRpm
    {
        get => _SpeedRpm;
    }

    //Constructors
    public FanController(DeviceId id)
    {
        Id = id;
    }

    //Methods
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

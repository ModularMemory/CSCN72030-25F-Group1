using FalloutVault.Devices.Interfaces;
using FalloutVault.Models;


namespace FalloutVault.Devices;
public class FanController : Device, IFanController
{
    //fields
    private bool _IsOn;

    private double _TargetRpm;

    private double _SpeedRpm;

    //properties
   
    public override string Name { get; }
    public override string Zone { get; }
    public override EventHandler<DeviceMessage>? OnDeviceMessage { get; set; }

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
    public FanController(string name, string zone)
    {
        Name = name;
        Zone = zone;
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

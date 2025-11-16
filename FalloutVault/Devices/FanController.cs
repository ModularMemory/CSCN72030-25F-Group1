using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class FanController : PoweredDevice, IFanController
{
    //fields
    private bool _IsOn;

    private double _TargetRpm;

    private double _SpeedRpm;

    //properties

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.FanController;

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
    }

    public override void SendCommand(DeviceCommand command)
    {
        throw new NotImplementedException();
    }

    protected override Watt ComputePowerDraw()
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

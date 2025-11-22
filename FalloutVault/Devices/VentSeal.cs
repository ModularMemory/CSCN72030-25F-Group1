using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class VentSealController : PoweredDevice, IVentSealController
{
    //properties
    private int _Section;

    private bool _IsOpen;

    private bool _LockState;

    //properties
    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.VentSealController;


    public int Section
    {
        get => _Section;
        set => _Section = value;
    }

    public bool LockState
    {
        get => _LockState;
        set => _LockState = value;
    }

    public bool IsOpen
    {
        get => _IsOpen;
        set
        {
            if (LockState == true) {
                //redundant placeholder
            }
            else
            {
                _IsOpen = value;
            }
        }
    }

    public VentSealController(DeviceId id)
    {
        Id = id;
    }

    public override void Update()
    {
    }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case DeviceCommand.IsOpen:
                IsOpen = (bool)command.Data!; break;
            case DeviceCommand.IsLocked:
                LockState = (bool)command.Data!; break;
            case DeviceCommand.CurrentSection:
                Section = (int)command.Data!; break;
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.VentStateChanged(IsOpen));
                PublishMessage(new DeviceMessage.VentLockedChanged(LockState));
                PublishMessage(new DeviceMessage.VentLockedChanged(Section));
                break;
        }
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


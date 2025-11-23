using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class VentSealController : Device, IVentSealController
{
    //properties
    private bool _IsOpen;

    private bool _LockState;

    //properties
    public override DeviceId Id { get; }

    public override DeviceType Type => DeviceType.VentSealController;


  
    public bool LockState
    {
        get => _LockState;

        set
        { _LockState = value;
            PublishMessage(new DeviceMessage.VentLockedChanged(field));
        }
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
                PublishMessage(new DeviceMessage.VentStateChanged(field));
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
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.VentStateChanged(IsOpen));
                PublishMessage(new DeviceMessage.VentLockedChanged(LockState));
                break;
        }
    }
}


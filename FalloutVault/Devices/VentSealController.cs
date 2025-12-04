using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class VentSealController : Device, IVentSealController
{
    //fields
    private bool _LockState;

    //properties
    public override DeviceId Id { get; }

    public override DeviceType Type => DeviceType.VentSealController;


    public bool LockState
    {
        get => _LockState;
        set
        {
            _LockState = value;
            PublishMessage(new DeviceMessage.VentLockedChanged(_LockState));
        }
    }

    public bool IsOpen
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.VentOpenChanged(field));
        }
    }
    

    public VentSealController(DeviceId id)
    {
        Id = id;
    }

    public override void Update() { }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case DeviceCommand.SetOpen setOpen:
                IsOpen = setOpen.IsOpen;
                break;
            case DeviceCommand.SetVentLocked setVentLocked:
                LockState = setVentLocked.IsLocked;
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.VentOpenChanged(IsOpen));
                PublishMessage(new DeviceMessage.VentLockedChanged(LockState));
                break;
        }
    }
}
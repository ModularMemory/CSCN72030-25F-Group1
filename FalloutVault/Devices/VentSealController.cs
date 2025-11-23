using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class VentSealController : Device, IVentSealController
{
    //fields
    private bool _IsOpen;
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
        get => _IsOpen;
        set
        {
            if (LockState)
                return;

            _IsOpen = value;
            PublishMessage(new DeviceMessage.VentStateChanged(_IsOpen));
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
            case DeviceCommand.SetVentOpen setVentOpen:
                IsOpen = setVentOpen.IsOpen;
                break;
            case DeviceCommand.SetVentLocked setVentLocked:
                LockState = setVentLocked.IsLocked;
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.VentStateChanged(IsOpen));
                PublishMessage(new DeviceMessage.VentLockedChanged(LockState));
                break;
        }
    }
}
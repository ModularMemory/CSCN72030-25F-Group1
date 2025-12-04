using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using static FalloutVault.Eventing.Models.DeviceMessage;


namespace FalloutVault.Devices;
public class DoorController : Device, IDoorController
{
    //Fields
    public override DeviceId Id { get; }

    public override DeviceType Type => DeviceType.VentSealController;

    //Properties
    public bool IsOpen
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.DoorOpenCloseChanged(field));
        }
    }

    public bool IsLocked
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.DoorLockChanged(field));
        }
    }

    //Constructors
    public DoorController(DeviceId id)
    {
        Id = id;
    }

    //Methods
    public override void Update() { }
    

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case DeviceCommand.SetOpen setDoorOpen:
                IsOpen = setDoorOpen.IsOpen;
                break;
            case DeviceCommand.SetDoorLocked setDoorLocked:
                IsLocked = setDoorLocked.IsLocked;
                break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.DoorOpenCloseChanged(IsOpen));
                PublishMessage(new DeviceMessage.DoorLockChanged(IsLocked));
                break;
        }
    }
}

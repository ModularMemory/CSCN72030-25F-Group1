namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class DoorOpenCloseChanged(bool data) : DeviceOpenCloseChanged(data)
    {
        public override string Message => "Door opened/closed changed";
    }

    public class DoorLockChanged(bool data) : DeviceLockedChanged(data)
    {
        public override string Message => "Door lock state changed";
    }
}

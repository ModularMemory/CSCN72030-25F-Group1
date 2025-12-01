namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class VentOpenChanged(bool data) : DeviceOpenCloseChanged(data)
    {
        public override string Message => "Vent open changed";
    }

    public class VentLockedChanged(bool data) : DeviceMessage(data)
    {
        public bool IsLocked { get; } = data;

        public override string Message => "Vent Lock state changed";
    }
}
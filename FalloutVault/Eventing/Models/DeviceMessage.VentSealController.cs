namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class VentOpenChanged(bool data) : DeviceOpenCloseChanged(data)
    {
        public override string Message => "Vent opened/closed changed";
    }

    public class VentLockedChanged(bool data) : DeviceLockedChanged(data)
    {
        public override string Message => "Vent lock state changed";
    }
}
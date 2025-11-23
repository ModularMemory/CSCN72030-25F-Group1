namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class VentStateChanged : DeviceOnChanged
    {
        public VentStateChanged(bool data) : base(data) { }
        public VentStateChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Vent state changed";
    }

    public class VentLockedChanged : DeviceMessage
    {
        public VentLockedChanged(bool data) : base(data) { }
        public VentLockedChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Vent Lock state changed";
    }
}
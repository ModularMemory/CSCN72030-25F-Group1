namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class LightOnChanged : DeviceOnChanged
    {
        public LightOnChanged(bool data) : base(data) { }
        public LightOnChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Light on changed";
    }

    public class DimmerLevelChanged : DeviceMessage
    {
        public DimmerLevelChanged(double data) : base(data) { }
        public DimmerLevelChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Light dimmer level changed";
    }
}
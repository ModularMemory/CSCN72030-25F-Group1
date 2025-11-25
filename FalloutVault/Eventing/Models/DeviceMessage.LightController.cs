namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class LightOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Light on changed";
    }

    public class DimmerLevelChanged(double data) : DeviceMessage(data)
    {
        public double DimmerLevel { get; } = data;

        public override string Message => "Light dimmer level changed";
    }
}
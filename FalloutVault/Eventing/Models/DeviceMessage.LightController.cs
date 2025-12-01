using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class LightBulbWattage(Watt data) : DeviceMessage(data)
    {
        public Watt Wattage { get; } = data;

        public override string Message => "Light bulb wattage";
    }

    public class LightOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Light on changed";
    }

    public class LightDimmerLevelChanged(double data) : DeviceMessage(data)
    {
        public double DimmerLevel { get; } = data;

        public override string Message => "Light dimmer level changed";
    }
}
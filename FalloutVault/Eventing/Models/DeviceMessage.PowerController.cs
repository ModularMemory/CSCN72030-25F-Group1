using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class PowerOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Power controller shutdown state changed";
    }

    public class PowerGenerationChanged(Watt data) : DeviceMessage(data)
    {
        public Watt PowerGeneration { get; } = data;

        public override string Message => "Power generation changed";
    }

    public class TotalPowerDrawChanged(PowerDraw data) : DeviceMessage(data)
    {
        public PowerDraw PowerDraw { get; } = data;

        public override string Message => "Total power draw changed";
    }
}
using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class PowerGenerationChanged : DeviceMessage
    {
        public PowerGenerationChanged(Watt data) : base(data) { }
        public PowerGenerationChanged(Watt data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Power generation changed";
    }

    public class TotalPowerDrawChanged : DeviceMessage
    {
        public TotalPowerDrawChanged(object? data) : base(data) { }
        public TotalPowerDrawChanged(object? data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Total power draw changed";
    }
}
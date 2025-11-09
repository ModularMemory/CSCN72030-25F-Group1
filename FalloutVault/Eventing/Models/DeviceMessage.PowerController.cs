using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class PowerGenerationChanged : DeviceMessage
    {
        public PowerGenerationChanged(Watt data) : base(data) { }
        public PowerGenerationChanged(DateTimeOffset timestamp, Watt data) : base(timestamp, data) { }

        public override string Message => "Power generation changed";
    }

    public class TotalPowerDrawChanged : DeviceMessage
    {
        public TotalPowerDrawChanged(object? data) : base(data) { }
        public TotalPowerDrawChanged(DateTimeOffset timestamp, object? data) : base(timestamp, data) { }

        public override string Message => "Total power draw changed";
    }
}
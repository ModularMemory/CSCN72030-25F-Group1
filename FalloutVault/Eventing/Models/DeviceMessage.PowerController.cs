using FalloutVault.Models;
using FalloutVault.Devices.Models;

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

    public class DeviceShutDownFromOverload(DeviceId data) : Alert(data)
    {
        public DeviceId DeviceId { get; } = data;
        public override string Message => "Device shut down due to power overload";
    }
}
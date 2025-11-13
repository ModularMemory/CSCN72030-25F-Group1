using FalloutVault.Utils;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public abstract class DeviceTurnedOn : DeviceMessage
    {
        public DeviceTurnedOn() : base(ValueBoxes.True) { }
        public DeviceTurnedOn(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.True) { }
    }

    public abstract class DeviceTurnedOff : DeviceMessage
    {
        public DeviceTurnedOff() : base(ValueBoxes.True) { }
        public DeviceTurnedOff(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.False) { }
    }
}
namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class DeviceTurnedOn : DeviceMessage
    {
        public DeviceTurnedOn() : base(ValueBoxes.True) { }
        public DeviceTurnedOn(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.True) { }

        public override string Message => "Device turned on";
    }

    public class DeviceTurnedOff : DeviceMessage
    {
        public DeviceTurnedOff() : base(ValueBoxes.True) { }
        public DeviceTurnedOff(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.False) { }

        public override string Message => "Device turned off";
    }
}
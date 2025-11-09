namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class LightTurnedOn : DeviceMessage
    {
        public LightTurnedOn() : base(ValueBoxes.True) { }
        public LightTurnedOn(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.True) { }

        public override string Message => "Light turned on";
    }

    public class LightTurnedOff : DeviceMessage
    {
        public LightTurnedOff() : base(ValueBoxes.False) { }
        public LightTurnedOff(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.False) { }

        public override string Message => "Light turned off";
    }

    public class DimmerLevelChanged : DeviceMessage
    {
        public DimmerLevelChanged(double data) : base(data) { }
        public DimmerLevelChanged(DateTimeOffset timestamp, double data) : base(timestamp, data) { }

        public override string Message => "Light dimmer level changed";
    }
}
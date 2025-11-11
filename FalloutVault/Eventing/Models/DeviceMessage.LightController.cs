namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class LightTurnedOn : DeviceTurnedOn
    {
        public LightTurnedOn() { }
        public LightTurnedOn(DateTimeOffset timestamp) : base(timestamp) { }

        public override string Message => "Light turned on";
    }

    public class LightTurnedOff : DeviceTurnedOff
    {
        public LightTurnedOff(){ }
        public LightTurnedOff(DateTimeOffset timestamp) : base(timestamp) { }

        public override string Message => "Light turned off";
    }

    public class DimmerLevelChanged : DeviceMessage
    {
        public DimmerLevelChanged(double data) : base(data) { }
        public DimmerLevelChanged(DateTimeOffset timestamp, double data) : base(timestamp, data) { }

        public override string Message => "Light dimmer level changed";
    }
}
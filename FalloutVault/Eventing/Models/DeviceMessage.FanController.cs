namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class FanOnChanged : DeviceOnChanged
    {
        public FanOnChanged(bool data) : base(data) { }
        public FanOnChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Fan on changed";
    }

    public class FanTargetRpmChanged : DeviceMessage
    {
        public FanTargetRpmChanged(double data) : base(data) { }
        public FanTargetRpmChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Fan target RPM changed";
    }

    public class FanSpeedRpmChanged : DeviceMessage
    {
        public FanSpeedRpmChanged(double data) : base(data) { }
        public FanSpeedRpmChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Fan speed RPM changed";
    }
}
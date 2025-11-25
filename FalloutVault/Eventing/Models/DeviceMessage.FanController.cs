namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class FanOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Fan on changed";
    }

    public class FanTargetRpmChanged(int data) : DeviceMessage(data)
    {
        public int TargetRpm { get; } = data;

        public override string Message => "Fan target RPM changed";
    }

    public class FanSpeedRpmChanged(int data) : DeviceMessage(data)
    {
        public int SpeedRpm { get; } = data;

        public override string Message => "Fan speed RPM changed";
    }
}
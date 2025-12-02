using System.ComponentModel.DataAnnotations;
using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class FanOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Fan on/off changed";
    }

    public class FanMotorWattage(Watt data) : DeviceMessage(data)
    {
        public Watt Wattage { get; } = data;

        public override string Message => "Fan motor wattage";
    }

    public class FanMaxRpm([Range(0, int.MaxValue)] int data) : DeviceMessage(data)
    {
        public int MaxRpm { get; } = data;

        public override string Message => "Fan max RPM";
    }

    public class FanTargetRpmChanged([Range(0, int.MaxValue)] int data) : DeviceMessage(data)
    {
        public int TargetRpm { get; } = data;

        public override string Message => "Fan target RPM changed";
    }

    public class FanSpeedRpmChanged([Range(0, int.MaxValue)] int data) : DeviceMessage(data)
    {
        public int SpeedRpm { get; } = data;

        public override string Message => "Fan speed RPM changed";
    }
}
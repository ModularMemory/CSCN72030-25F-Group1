
using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Speaker on/off changed";
    }

    public class VolumeLevelChanged(double data) : DeviceMessage(data)
    {
        public double Volume { get; } = data;

        public override string Message => "Speaker volume level changed";
    }

    public class SpeakerWattage(Watt data) : DeviceMessage(data)
    {
        public Watt wattage { get; } = data;

        public override string Message => "Speaker wattage";
    }
    
}
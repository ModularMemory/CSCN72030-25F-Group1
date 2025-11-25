namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Speaker on changed";
    }

    public class VolumeLevelChanged(double data) : DeviceMessage(data)
    {
        public double Volume { get; } = data;

        public override string Message => "Speaker volume level changed";
    }
    
}
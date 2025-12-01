
namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerOnChanged : DeviceOnChanged
    {
        public SpeakerOnChanged(bool data) : base(data) { }
        public SpeakerOnChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Speaker on changed";
    }

    public class VolumeLevelChanged : DeviceMessage
    {
        public VolumeLevelChanged(double data) : base(data) { }
        public VolumeLevelChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Speaker volume level changed";
    }
    
}
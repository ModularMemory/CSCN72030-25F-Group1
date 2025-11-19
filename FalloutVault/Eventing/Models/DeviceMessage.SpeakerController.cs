namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerOnChanged : DeviceOnChanged
    {
        public SpeakerOnChanged(bool data) : base(data) { }
        public SpeakerOnChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Speaker on changed";
    }
}
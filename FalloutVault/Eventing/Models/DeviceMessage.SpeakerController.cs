namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerTurnedOn : DeviceTurnedOn
    {
        public SpeakerTurnedOn() { }
        public SpeakerTurnedOn(DateTimeOffset timestamp) : base(timestamp) { }

        public override string Message => "Speaker turned on";
    }

    public class SpeakerTurnedOff : DeviceTurnedOff
    {
        public SpeakerTurnedOff() { }
        public SpeakerTurnedOff(DateTimeOffset timestamp) : base(timestamp) { }

        public override string Message => "Speaker turned off";
    }
}
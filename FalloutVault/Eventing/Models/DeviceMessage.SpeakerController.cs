namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class SpeakerTurnedOn : DeviceMessage
    {
        public SpeakerTurnedOn() : base(ValueBoxes.True) { }
        public SpeakerTurnedOn(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.True) { }

        public override string Message => "Speaker turned on";
    }

    public class SpeakerTurnedOff : DeviceMessage
    {
        public SpeakerTurnedOff() : base(ValueBoxes.False) { }
        public SpeakerTurnedOff(DateTimeOffset timestamp) : base(timestamp, ValueBoxes.False) { }

        public override string Message => "Speaker turned off";
    }
}
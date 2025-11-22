namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetSpeakerVolume : DeviceCommand
    {
        public SetSpeakerVolume(double data) : base(data) { }
        public SetSpeakerVolume(double data, DateTimeOffset timestamp) : base(data, timestamp) { }
    }
}
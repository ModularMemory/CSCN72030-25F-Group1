namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetSpeakerVolume(double data) : DeviceCommand(data)
    {
        public double Volume { get; } = data;
    }
}
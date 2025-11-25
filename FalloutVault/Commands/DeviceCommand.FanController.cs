namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetFanTargetRpm(int data) : DeviceCommand(data)
    {
        public int TargetRpm { get; } = data;
    }
}
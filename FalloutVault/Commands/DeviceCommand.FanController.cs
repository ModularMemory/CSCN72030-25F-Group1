namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetFanTargetRpm(double data) : DeviceCommand(data)
    {
        public double TargetRpm { get; } = data;
    }
}
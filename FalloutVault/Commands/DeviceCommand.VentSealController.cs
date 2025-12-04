namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetVentLocked(bool data) : DeviceCommand(data)
    {
        public bool IsLocked { get; } = data;
    }
}
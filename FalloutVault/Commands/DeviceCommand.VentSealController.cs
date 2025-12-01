namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetVentOpen(bool data) : DeviceCommand(data)
    {
        public bool IsOpen { get; } = data;
    }

    public class SetVentLocked(bool data) : DeviceCommand(data)
    {
        public bool IsLocked { get; } = data;
    }
}
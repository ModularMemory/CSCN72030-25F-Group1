using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetDoorLocked(bool data) : DeviceCommand(data)
    {
        public bool IsLocked { get; } = data;
    }
}
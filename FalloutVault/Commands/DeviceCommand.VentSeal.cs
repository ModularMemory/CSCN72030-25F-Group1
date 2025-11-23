using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class IsOpen( bool data) : DeviceCommand(data);

    public class IsLocked (bool data) : DeviceCommand(data);

    public class CurrentSection(int data) : DeviceCommand(data);
}

//IsOpen, IsLocked, CurrentSection
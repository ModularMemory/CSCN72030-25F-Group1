using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class IsOn( bool data) : DeviceCommand(data);
    public class CurrentCropSection(int data) : DeviceCommand(data);
}

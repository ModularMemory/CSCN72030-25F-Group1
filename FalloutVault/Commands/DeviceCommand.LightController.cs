using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetLightDimmer([Range(0, 1)] double data) : DeviceCommand(data);
}
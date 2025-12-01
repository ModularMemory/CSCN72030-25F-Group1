namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class CurrentCropSection(int data) : DeviceCommand(data)
    {
        public int Section { get; } = data;
    }
}

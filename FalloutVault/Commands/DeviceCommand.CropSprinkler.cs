using FalloutVault.Models;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetCropSection(SprinklerSection data) : DeviceCommand(data)
    {
        public SprinklerSection Section { get; } = data;
    }

    public class SetCropTargetLitres(int data) : DeviceCommand(data)
    {
        public int Litres { get; } = data;
    }
}

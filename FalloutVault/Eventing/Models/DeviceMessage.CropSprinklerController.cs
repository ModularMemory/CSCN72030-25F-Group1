using FalloutVault.Models;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class CropSprinklerWattage(Watt data) : DeviceMessage(data)
    {
        public Watt Wattage { get; } = data;

        public override string Message => "Crop sprinkler wattage";
    }

    public class CropSprinklerOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Crop sprinkler on/off changed";
    }

    public class CropSprinklerSectionChanged(SprinklerSection data) : DeviceMessage(data)
    {
        public SprinklerSection Section { get; } = data;

        public override string Message => "Crop sprinkler target section changed";
    }

    public class CropSprinklerTargetLitresChanged(int data) : DeviceMessage(data)
    {
        public int TargetLitres { get; } = data;

        public override string Message => "Crop sprinkler target litres changed";
    }

    public class CropSprinklerTimeOnChanged(TimeSpan data) : DeviceMessage(data)
    {
        public TimeSpan TimeOn { get; } = data;

        public override string Message => "Crop sprinkler time on changed";
    }
}
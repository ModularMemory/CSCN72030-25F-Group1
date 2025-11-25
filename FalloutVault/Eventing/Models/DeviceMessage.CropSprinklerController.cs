namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class CropSprinklerOnOffChanged(bool data) : DeviceOnOffChanged(data)
    {
        public override string Message => "Crop Sprinkler state changed";
    }

    public class CropSprinklerSectionChanged(int data) : DeviceMessage(data)
    {
        public int Section { get; } = data;

        public override string Message => "Crop Sprinkler Target Section changed";
    }
}
namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class CropSprinklerStateChanged : DeviceOnChanged
    {
        public CropSprinklerStateChanged(bool data) : base(data) { }
        public CropSprinklerStateChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Crop Sprinkler state changed";
    }

    public class CropSprinklerSectionChanged : DeviceMessage
    {
        public CropSprinklerSectionChanged(double data) : base(data) { }
        public CropSprinklerSectionChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Crop Sprinkler Target Section changed";
    }
}
namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public class CropSprinklerOnChanged : DeviceOnChanged
    {
        public LightOnChanged(bool data) : base(data) { }
        public LightOnChanged(bool data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Crop Sprinkler state changed";
    }

    public class DimmerLevelChanged : DeviceMessage
    {
        public CropSprinklerSectionChanged(double data) : base(data) { }
        public CropSprinklerSectionChanged(double data, DateTimeOffset timestamp) : base(data, timestamp) { }

        public override string Message => "Crop Sprinkler Target Section changed";
    }
}
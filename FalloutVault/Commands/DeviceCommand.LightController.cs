namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class SetLightDimmer : DeviceCommand
    {
        public SetLightDimmer(double data) : base(data) { }
        public SetLightDimmer(double data, DateTimeOffset timestamp) : base(data, timestamp) { }
    }
}
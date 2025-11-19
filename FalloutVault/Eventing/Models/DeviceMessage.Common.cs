using FalloutVault.Utils;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public abstract class DeviceOnChanged : DeviceMessage
    {
        public DeviceOnChanged(bool data) : base(ValueBoxes.BooleanBox(data)) { }
        public DeviceOnChanged(bool data, DateTimeOffset timestamp) : base(ValueBoxes.BooleanBox(data), timestamp) { }
    }
}
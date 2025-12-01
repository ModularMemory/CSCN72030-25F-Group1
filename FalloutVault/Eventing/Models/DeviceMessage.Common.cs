using FalloutVault.Utils;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public abstract class DeviceOnOffChanged(bool data) : DeviceMessage(ValueBoxes.BooleanBox(data))
    {
        public bool IsOn { get; } = data;
    }

    public abstract class DeviceOpenCloseChanged(bool data) : DeviceMessage(ValueBoxes.BooleanBox(data))
    {
        public bool IsOpen { get; } = data;
    }
}
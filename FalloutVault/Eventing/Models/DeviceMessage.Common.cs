using FalloutVault.Utils;

namespace FalloutVault.Eventing.Models;

public partial class DeviceMessage
{
    public abstract class Alert(object data) : DeviceMessage(data);

    public abstract class DeviceOnOffChanged(bool data) : DeviceMessage(ValueBoxes.BooleanBox(data))
    {
        public bool IsOn { get; } = data;
    }

    public abstract class DeviceTimedOnOffChanged(TimeSpan data) : DeviceMessage(data)
    {
        public TimeSpan TimeRemaining { get; } = data;
    }

    public abstract class DeviceOpenCloseChanged(bool data) : DeviceMessage(ValueBoxes.BooleanBox(data))
    {
        public bool IsOpen { get; } = data;
    }

    public abstract class DeviceLockedChanged(bool data) : DeviceMessage(ValueBoxes.BooleanBox(data))
    {
        public bool IsLocked { get; } = data;
    }
}
using FalloutVault.Utils;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class GetCurrentState() : DeviceCommand(null);

    // IOnOff
    public class SetOn(bool data) : DeviceCommand(ValueBoxes.BooleanBox(data))
    {
        public bool IsOn { get; } = data;
    }

    // ITemporaryOn
    public class TurnOnFor(TimeSpan data) : DeviceCommand(data)
    {
        public TimeSpan Time { get; } = data;
    }

    // ITemporaryOff
    public class TurnOffFor(TimeSpan data) : DeviceCommand(data)
    {
        public TimeSpan Time { get; } = data;
    }
}
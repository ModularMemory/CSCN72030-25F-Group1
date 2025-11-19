using FalloutVault.Utils;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    // IOnOff
    public class SetOn : DeviceCommand
    {
        public SetOn(bool data) : base(ValueBoxes.BooleanBox(data)) { }
        public SetOn(bool data, DateTimeOffset timestamp) : base(ValueBoxes.BooleanBox(data), timestamp) { }
    }

    // ITemporaryOn
    public class TurnOnFor : DeviceCommand
    {
        public TurnOnFor(TimeSpan? data) : base(data) { }
        public TurnOnFor(TimeSpan? data, DateTimeOffset timestamp) : base(data, timestamp) { }
    }

    // ITemporaryOff
    public class TurnOffFor : DeviceCommand
    {
        public TurnOffFor(TimeSpan? data) : base(data) { }
        public TurnOffFor(TimeSpan? data, DateTimeOffset timestamp) : base(data, timestamp) { }
    }
}
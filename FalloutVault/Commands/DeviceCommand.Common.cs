using FalloutVault.Utils;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class GetCurrentState() : DeviceCommand(null);

    // IOnOff
    public class SetOn(bool data) : DeviceCommand(ValueBoxes.BooleanBox(data));

    // ITemporaryOn
    public class TurnOnFor(TimeSpan? data) : DeviceCommand(data);

    // ITemporaryOff
    public class TurnOffFor(TimeSpan? data) : DeviceCommand(data);
}
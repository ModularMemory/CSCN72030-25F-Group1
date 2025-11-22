using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Commands;

public partial class DeviceCommand
{
    public class RequestPower(DeviceId deviceId, Watt amount) : DeviceCommand((deviceId, amount))
    {
        public DeviceId DeviceId { get; } = deviceId;
        public Watt Amount { get; } = amount;
    }

    public class ReleasePower(DeviceId deviceId) : DeviceCommand(deviceId)
    {
        public DeviceId DeviceId { get; } = deviceId;
    }
}
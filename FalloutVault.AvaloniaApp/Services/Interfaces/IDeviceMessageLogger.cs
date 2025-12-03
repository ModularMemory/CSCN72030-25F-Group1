using FalloutVault.AvaloniaApp.Models;

namespace FalloutVault.AvaloniaApp.Services.Interfaces;

public interface IDeviceMessageLogger
{
    IReadOnlyList<DeviceLog> Messages { get; }
    event EventHandler<DeviceLog>? DeviceMessageReceived;
}
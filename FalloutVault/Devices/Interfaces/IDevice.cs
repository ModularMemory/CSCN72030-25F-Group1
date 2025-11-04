using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IDevice
{
    /// <summary>
    /// The name of the device.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The zone where the device is located.
    /// </summary>
    string Zone { get; }

    /// <summary>
    /// Device message event handler.
    /// </summary>
    EventHandler<DeviceMessage> OnDeviceMessage { get; }

    /// <summary>
    /// Triggers a manual update check.
    /// </summary>
    void Update();
}
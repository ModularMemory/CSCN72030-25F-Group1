using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Interfaces;

public interface IDeviceRegistry
{
    /// <summary>
    /// Fires when a new device is added to the registry.
    /// </summary>
    event EventHandler<IDevice>? DeviceRegistered;

    /// <summary>
    /// The number of registered devices.
    /// </summary>
    int DeviceCount { get; }

    /// <summary>
    /// The <see cref="DeviceId"/>, <see cref="DeviceType"/>, and <see cref="DeviceCapabilities"/> of the devices in the registry.
    /// </summary>
    IEnumerable<(DeviceId deviceId, DeviceType deviceType, DeviceCapabilities deviceCapabilities)> Devices { get; }

    /// <summary>
    /// Registers a new device with the registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The registry instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    IDeviceRegistry RegisterDevice<TDevice>(TDevice device) where TDevice : IDevice;

    /// <summary>
    /// Tries to get the type and capabilities of the requested device.
    /// </summary>
    /// <param name="deviceId">The device to query for.</param>
    /// <param name="deviceType">The type of the device.</param>
    /// <param name="deviceCapabilities">The capabilities of the device.</param>
    /// <returns>True if the device info was found in the registry, otherwise false.</returns>
    bool TryGetDeviceInfo(DeviceId deviceId, out DeviceType deviceType, out DeviceCapabilities deviceCapabilities);

    /// <summary>
    /// Gets the type and capabilities of a given device.
    /// </summary>
    /// <param name="deviceId">The device to search for.</param>
    /// <exception cref="KeyNotFoundException">The <paramref name="deviceId"/> is not in the registry.</exception>
    (DeviceType deviceType, DeviceCapabilities deviceCapabilities) this[DeviceId deviceId] { get; }
}
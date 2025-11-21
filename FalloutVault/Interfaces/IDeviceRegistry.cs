using System.Diagnostics.CodeAnalysis;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Interfaces;

public interface IDeviceRegistry
{
    /// <summary>
    /// The device instances registered in the registry.
    /// </summary>
    internal IEnumerable<IDevice> DeviceInstances { get; }

    /// <summary>
    /// Fires when a new device is added to the registry.
    /// </summary>
    internal event EventHandler<IDevice>? DeviceRegistered;

    /// <summary>
    /// The number of registered devices.
    /// </summary>
    int DeviceCount { get; }

    /// <summary>
    /// The <see cref="DeviceId"/>, <see cref="DeviceType"/>, and <see cref="DeviceCapabilities"/> of the devices in the registry.
    /// </summary>
    IEnumerable<(DeviceId id, DeviceType type, DeviceCapabilities capabilities)> Devices { get; }

    /// <summary>
    /// Tries to get the registered device by the given id.
    /// </summary>
    /// <param name="deviceId">The device to query for.</param>
    /// <param name="device">The device instance.</param>
    /// <returns>True if the device was found in the registry, otherwise false.</returns>
    internal bool TryGetDeviceInstance(DeviceId deviceId, [NotNullWhen(true)] out IDevice? device);

    /// <summary>
    /// Registers a new device with the registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same <see cref="IDevice.Id"/> was already added.</exception>
    IDeviceRegistry RegisterDevice(IDevice device);

    /// <summary>
    /// Tries to get the type and capabilities of the requested device.
    /// </summary>
    /// <param name="id">The device to query for.</param>
    /// <param name="type">The type of the device.</param>
    /// <param name="capabilities">The capabilities of the device.</param>
    /// <returns>True if the device info was found in the registry, otherwise false.</returns>
    bool TryGetDeviceInfo(DeviceId id, out DeviceType type, out DeviceCapabilities capabilities);

    /// <summary>
    /// Gets the type and capabilities of a given device.
    /// </summary>
    /// <param name="deviceId">The device to search for.</param>
    /// <exception cref="KeyNotFoundException">The <paramref name="deviceId"/> is not in the registry.</exception>
    (DeviceType type, DeviceCapabilities capabilities) this[DeviceId deviceId] { get; }
}
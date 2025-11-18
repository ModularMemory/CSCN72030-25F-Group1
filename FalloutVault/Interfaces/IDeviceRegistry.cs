using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Interfaces;

public interface IDeviceRegistry
{
    internal IEnumerable<IDevice> DeviceInstancesInternal { get; }

    int DeviceCount { get; }
    IEnumerable<(DeviceId deviceId, DeviceType deviceType, DeviceCapabilities deviceCapabilities)> Devices { get; }
    (DeviceType deviceType, DeviceCapabilities deviceCapabilities) this[DeviceId deviceId] { get; }

    /// <summary>
    /// Registers a new device with the registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The registry instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    DeviceRegistry RegisterDevice<TDevice>(TDevice device) where TDevice : IDevice;
}
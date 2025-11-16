using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceRegistry
{
    private readonly ILogger _logger;
    private readonly Dictionary<DeviceId, (IDevice device, DeviceType deviceType, DeviceCapabilities deviceCapabilities)> _devices = [];

    internal IEnumerable<IDevice> Devices => _devices.Values.Select(x => x.device);

    public DeviceRegistry(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Registers a new device with the registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The registry instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    internal DeviceRegistry RegisterDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        var deviceId = device.Id;
        var deviceType = device.Type;
        var capabilities = BuildDeviceCapabilities(device);

        _devices.Add(deviceId, (device, deviceType, capabilities));

        _logger.Information("Registered device {DeviceId} with type {DeviceType} and capabilities {DeviceCapabilities}", deviceId, deviceType, capabilities);

        return this;
    }

    private static DeviceCapabilities BuildDeviceCapabilities<TDevice>(TDevice device) where TDevice : IDevice
    {
        var capabilities = DeviceCapabilities.None;

        if (device is IOnOff) capabilities |= DeviceCapabilities.OnOff;
        if (device is ITemporaryOff) capabilities |= DeviceCapabilities.TemporaryOff;
        if (device is ITemporaryOn) capabilities |= DeviceCapabilities.TemporaryOn;
        if (device is IPeriodic) capabilities |= DeviceCapabilities.Periodic;

        return capabilities;
    }
}
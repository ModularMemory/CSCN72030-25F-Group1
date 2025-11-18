using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceRegistry : IDeviceRegistry
{
    private readonly ILogger _logger;
    private readonly Dictionary<DeviceId, (IDevice device, DeviceType deviceType, DeviceCapabilities deviceCapabilities)> _devices = [];

    public IEnumerable<IDevice> DeviceInstancesInternal => _devices.Values.Select(x => x.device);

    public int DeviceCount => _devices.Count;
    public IEnumerable<(DeviceId deviceId, DeviceType deviceType, DeviceCapabilities deviceCapabilities)> Devices
        => _devices.Select(x => (x.Key, x.Value.deviceType, x.Value.deviceCapabilities));

    public (DeviceType deviceType, DeviceCapabilities deviceCapabilities) this[DeviceId deviceId]
    {
        get
        {
            var device = _devices[deviceId];
            return (device.deviceType, device.deviceCapabilities);
        }
    }

    public DeviceRegistry(ILogger logger)
    {
        _logger = logger;
    }

    public DeviceRegistry RegisterDevice<TDevice>(TDevice device) where TDevice : IDevice
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
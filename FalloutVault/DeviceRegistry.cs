using System.Diagnostics.CodeAnalysis;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceRegistry : IDeviceRegistry
{
    private readonly ILogger _logger;
    private readonly Dictionary<DeviceId, (IDevice device, DeviceType type, DeviceCapabilities capabilities)> _devices = [];

    public IEnumerable<IDevice> DeviceInstances => _devices.Values.Select(x => x.device);

    public event EventHandler<IDevice>? DeviceRegistered;

    public int DeviceCount => _devices.Count;

    public IEnumerable<(DeviceId id, DeviceType type, DeviceCapabilities capabilities)> Devices
        => _devices.Select(x => (x.Key, deviceType: x.Value.type, deviceCapabilities: x.Value.capabilities));

    public DeviceRegistry(ILogger logger)
    {
        _logger = logger;
    }

    public bool TryGetDeviceInstance(DeviceId deviceId, [NotNullWhen(true)] out IDevice? device)
    {
        if (_devices.TryGetValue(deviceId, out var info))
        {
            device = info.device;
            return true;
        }

        device = null;
        return false;
    }

    public IDeviceRegistry RegisterDevice(IDevice device)
    {
        var deviceId = device.Id;
        var deviceType = device.Type;
        var capabilities = BuildDeviceCapabilities(device);

        _devices.Add(deviceId, (device, deviceType, capabilities));

        _logger.Information("Registered device {DeviceId} with type {DeviceType} and capabilities {DeviceCapabilities}", deviceId, deviceType, capabilities);

        OnDeviceRegistered(device);

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

    public bool TryGetDeviceInfo(DeviceId id, out DeviceType type, out DeviceCapabilities capabilities)
    {
        if (_devices.TryGetValue(id, out var device))
        {
            (type, capabilities) = (device.type, device.capabilities);
            return true;
        }

        type = default;
        capabilities = default;
        return false;
    }

    public (DeviceType type, DeviceCapabilities capabilities) this[DeviceId deviceId]
    {
        get
        {
            var device = _devices[deviceId];
            return (device.type, device.capabilities);
        }
    }

    private void OnDeviceRegistered(IDevice e)
    {
        DeviceRegistered?.Invoke(this, e);
    }
}
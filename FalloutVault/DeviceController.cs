using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceController : IDeviceController, IDisposable
{
    private readonly ILogger _logger;
    private readonly IDeviceRegistry _deviceRegistry;
    private readonly IEventBus<DeviceMessage> _messageBus;
    private readonly IEventBus<Watt> _powerEventBus;

    private Timer? _pollTimer;

    public DeviceController(IDeviceRegistry deviceRegistry, IEventBus<DeviceMessage> messageBus, IEventBus<Watt> powerEventBus, ILogger logger)
    {
        _deviceRegistry = deviceRegistry;
        _messageBus = messageBus;
        _powerEventBus = powerEventBus;
        _logger = logger;

        _deviceRegistry.DeviceRegistered += DeviceRegistryOnDeviceRegistered;
        foreach (var device in _deviceRegistry.DeviceInstances)
        {
            // Ensure all devices are accounted for
            DeviceRegistryOnDeviceRegistered(null, device);
        }
    }

    private void DeviceRegistryOnDeviceRegistered(object? sender, IDevice device)
    {
        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);
    }

    public void Start(TimeSpan pollingInterval)
    {
        if (_pollTimer is not null)
        {
            _logger.Warning("Tried to start controller while it was already running");
            return;
        }

        _logger.Information("Starting controller with polling interval: {Interval}", pollingInterval);

        _pollTimer = new Timer(PollTimerCallback, null, TimeSpan.Zero, pollingInterval);
    }

    private void PollTimerCallback(object? state)
    {
        foreach (var device in _deviceRegistry.DeviceInstances)
        {
            try
            {
                device.Update();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating device: {DeviceId}", device.Id);
            }
        }
    }

    public void Stop()
    {
        if (_pollTimer is null)
        {
            return;
        }

        _pollTimer.Dispose();
        _pollTimer = null;

        _logger.Information("Stopped controller");
    }

    public bool SendCommand(DeviceId targetDevice, DeviceCommand command)
    {
        if (_deviceRegistry.TryGetDeviceInstance(targetDevice, out var device))
        {
            device.SendCommand(command);
            return true;
        }

        return false;
    }

    public void Dispose()
    {
        _pollTimer?.Dispose();
    }
}
using FalloutVault.Devices.Interfaces;
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

    private Timer? _pollTimer;

    private readonly IEventBus<DeviceMessage> _messageBus;
    private readonly IEventBus<Watt> _powerEventBus;

    public DeviceController(IDeviceRegistry deviceRegistry, IEventBus<DeviceMessage> messageBus, IEventBus<Watt> powerEventBus, ILogger logger)
    {
        _deviceRegistry = deviceRegistry;
        _logger = logger;
        _messageBus = messageBus;
        _powerEventBus = powerEventBus;
    }

    public DeviceController AddDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        _deviceRegistry.RegisterDevice(device);

        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);

        _logger.Information("Connected device {DeviceId}", device.Id);

        return this;
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
        foreach (var device in _deviceRegistry.DeviceInstancesInternal)
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

    public void Dispose()
    {
        _pollTimer?.Dispose();
    }
}
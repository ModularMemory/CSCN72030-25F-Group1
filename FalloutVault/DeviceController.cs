using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceController : IDisposable
{
    private readonly ILogger _logger;
    private readonly DeviceRegistry _deviceRegistry;

    private Timer? _pollTimer;

    public IEventBus<DeviceMessage> MessageBus { get; }
    public IEventBus<Watt> PowerEventBus { get; }

    public DeviceController(DeviceRegistry deviceRegistry, ILogger logger)
    {
        _deviceRegistry = deviceRegistry;
        _logger = logger;
        MessageBus = new DeviceMessageEventBus();
        PowerEventBus = new PowerEventBus();
    }

    /// <summary>
    /// Registers a new device with the controller and the related registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The controller instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    public DeviceController AddDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        _deviceRegistry.RegisterDevice(device);

        device.SetEventBus(MessageBus);
        device.SetEventBus(PowerEventBus);

        _logger.Information("Connected device {DeviceId}", device.Id);

        return this;
    }

    /// <summary>
    /// Starts the controller with the default polling interval of 50ms.
    /// </summary>
    public void Start() => Start(TimeSpan.FromMilliseconds(50));

    /// <summary>
    /// Starts the controller with a custom polling interval.
    /// </summary>
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
        foreach (var device in _deviceRegistry.Devices)
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

    /// <summary>
    /// Stops the controller.
    /// </summary>
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
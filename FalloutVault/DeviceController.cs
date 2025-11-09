using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault;

public sealed class DeviceController : IDisposable
{
    // Fields

    private readonly ILogger _logger;
    private readonly Dictionary<DeviceId, IDevice> _devices = [];
    private readonly DeviceEventBus _messageBus = new();
    private readonly PowerEventBus _powerEventBus = new();

    private Timer? _pollTimer;

    // Properties

    public IReadOnlyCollection<IDevice> Devices => _devices.Values;
    public IEventBus<DeviceMessage> MessageBus => _messageBus;
    public IEventBus<Watt> PowerEventBus => _powerEventBus;

    // Constructors
    public DeviceController(ILogger logger)
    {
        _logger = logger;
    }

    // Methods

    /// <summary>
    /// Registers a new device with the controller.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The controller instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    public DeviceController AddDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        _devices.Add(device.Id, device);

        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);

        _logger.Information("Added new device: {DeviceId}", device.Id);

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
        if (_pollTimer is null)
        {
            return;
        }

        _logger.Information("Starting controller with polling interval: {Interval}", pollingInterval);

        _pollTimer = new Timer(PollTimerCallback, null, TimeSpan.Zero, pollingInterval);
    }

    private void PollTimerCallback(object? state)
    {
        foreach (var (_, device) in _devices)
        {
            device.Update();
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
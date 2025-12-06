using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.Services.Interfaces;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.Services;

public class DeviceMessageLogger : IDeviceMessageLogger
{
    private const int MAX_MESSAGES = 500;

    private readonly ILogger _logger;
    private readonly IEventBus<DeviceMessage> _messageBus;
    private readonly List<DeviceLog> _messages = [];
    private readonly Lock _messageLock = new();

    public IReadOnlyList<DeviceLog> Messages => _messages;
    public event EventHandler<DeviceLog>? DeviceMessageReceived;

    public DeviceMessageLogger(ILogger logger, IEventBus<DeviceMessage> messageBus)
    {
        _logger = logger;
        _messageBus = messageBus;
        _messageBus.Handler += MessageBus_OnDeviceMessage;
    }

    private void MessageBus_OnDeviceMessage(object? sender, DeviceMessage e)
    {
        var senderDevice = sender as IDevice;
        var senderString = senderDevice?.Id.ToString() ?? sender?.ToString();
        _logger.Information("Device message from {Sender}: {@Message}", senderString, e);
        // The @ in @Message means to JSON serialize the object rather than use .ToString()

        if (senderDevice is null ||
            e is DeviceMessage.FanSpeedRpmChanged
                or DeviceMessage.TotalPowerDrawChanged
                or DeviceMessage.DeviceTimedOnOffChanged
                or DeviceMessage.CropSprinklerTimeOnChanged)
        {
            return;
        }

        var newLog = new DeviceLog(senderDevice.Id, e);
        lock (_messageLock)
        {
            _messages.Add(newLog);
            while (_messages.Count > MAX_MESSAGES)
            {
                // This is bad, a ring buffer would be better, but I'm out of time
                _messages.RemoveAt(0);
            }
        }

        DeviceMessageReceived?.Invoke(sender, newLog);
    }
}
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using Microsoft.Extensions.Logging;

namespace FalloutVault.AvaloniaApp.Services;
public class DeviceMessageLogger
{
    private readonly ILogger _logger;
    private readonly IEventBus<DeviceMessage> _messageBus;

    public DeviceMessageLogger(ILogger logger, IEventBus<DeviceMessage> messageBus)
    {
        _logger = logger;
        _messageBus = messageBus;
    }
}

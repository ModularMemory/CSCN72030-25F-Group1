using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public abstract class PoweredDeviceViewModel : DeviceViewModel
{
    private readonly IEventBus<Watt> _powerBus;
    protected PoweredDeviceViewModel(IDeviceController deviceController, IEventBus<DeviceMessage> messageBus, IEventBus<Watt> powerBus, ILogger logger) : base(deviceController, messageBus, logger)
    {
        _powerBus = powerBus;
        _powerBus.Handler += OnPowerMessage;
    }

    protected abstract void OnPowerMessage(object? sender, Watt watts);

    ~PoweredDeviceViewModel()
    {
        _powerBus.Handler -= OnPowerMessage;
    }
}
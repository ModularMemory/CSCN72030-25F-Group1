using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public abstract partial class PoweredDeviceViewModel : DeviceViewModel
{
    private readonly IEventBus<Watt> _powerBus;

    [ObservableProperty]
    public partial Watt PowerDraw { get; set; }

    protected PoweredDeviceViewModel(IDeviceController deviceController, IEventBus<DeviceMessage> messageBus, IEventBus<Watt> powerBus, ILogger logger) : base(deviceController, messageBus, logger)
    {
        _powerBus = powerBus;
        _powerBus.Handler += OnPowerMessage;
    }

    private void OnPowerMessage(object? sender, Watt watts)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        PowerDraw = watts;
    }

    ~PoweredDeviceViewModel()
    {
        _powerBus.Handler -= OnPowerMessage;
    }
}
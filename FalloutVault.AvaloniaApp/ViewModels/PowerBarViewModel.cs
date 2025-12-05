using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FalloutVault.AvaloniaApp.ViewModels.Devices;
using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Material.Icons;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class PowerBarViewModel : ViewModelBase
{
    private readonly IEventBus<DeviceMessage> _messageBus;
    private readonly ILogger _logger;

    public PowerBarViewModel(IEventBus<DeviceMessage> messageBus, ILogger logger)
    {
        _messageBus = messageBus;
        _logger = logger;
        _messageBus.Handler += OnDeviceMessage;
    }

    [ObservableProperty]
    public partial Watt PowerDraw { get; set; }

    [ObservableProperty]
    public partial Watt PowerGeneration { get; set; }

    [ObservableProperty]
    public partial Watt PowerAvailable { get; set; }

    [ObservableProperty]
    public partial string? WarningMessage { get; set; } = null;

    [ObservableProperty]
    public partial MaterialIconKind? WarningIcon { get; set; } = null;

    private void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            switch (message)
            {
                case DeviceMessage.PowerGenerationChanged powerGenerationChanged:
                    PowerGeneration = powerGenerationChanged.PowerGeneration;
                    break;
                case DeviceMessage.TotalPowerDrawChanged powerDraw:
                    PowerDraw = powerDraw.PowerDraw.TotalDraw;
                    PowerAvailable = powerDraw.PowerDraw.Available;
                    if (PowerDraw > PowerGeneration * 0.8)
                    {
                        WarningMessage = "Warning: High Power Consumption";
                        WarningIcon = MaterialIconKind.AlertOutline;
                    }
                    else
                    {
                        WarningMessage = null;
                        WarningIcon = null;
                    }

                    break;
            }
        });
    }

    ~PowerBarViewModel()
    {
        _messageBus.Handler -= OnDeviceMessage;
    }
}
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using Material.Icons;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public partial class VentSealControllerViewModel : DeviceViewModel
{
    public VentSealControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus,
        ILogger logger)
        : base(deviceController, messageBus, logger) { }

    [ObservableProperty]
    public partial bool IsOpen { get; set; }

    [ObservableProperty]
    public partial bool IsLocked { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? OpenButtonColour { get; set; }

    [ObservableProperty]
    public partial MaterialIconKind VentIcon { get; set; }

    [ObservableProperty]
    public partial MaterialIconKind LockIcon { get; set; }

    [RelayCommand]
    public void SealButton_OnClick()
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetOpen(!IsOpen));
    }

    [RelayCommand]
    public void LockButton_OnClick()
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetVentLocked(!IsLocked));
    }

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        Dispatcher.UIThread.Invoke(() =>
        {
            switch (message)
            {
                case DeviceMessage.VentOpenChanged openChanged:
                    IsOpen = openChanged.IsOpen;
                    OpenButtonColour = new SolidColorBrush(IsOpen
                        ? Color.FromRgb(0, 255, 0)
                        : Color.FromRgb(255, 0, 0));
                    VentIcon = IsOpen
                        ? MaterialIconKind.Hvac
                        : MaterialIconKind.HvacOff;
                    break;
                case DeviceMessage.VentLockedChanged lockedChanged:
                    IsLocked = lockedChanged.IsLocked;
                    LockIcon = IsLocked
                        ? MaterialIconKind.Lock
                        : MaterialIconKind.LockOpenVariant;
                    break;
            }
        });
    }
}
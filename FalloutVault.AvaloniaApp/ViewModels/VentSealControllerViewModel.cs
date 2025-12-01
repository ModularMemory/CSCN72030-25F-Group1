using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using Material.Icons;
using Material.Icons.Avalonia;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class VentSealControllerViewModel : DeviceViewModel
{
    public VentSealControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus)
        : base(deviceController, messageBus) { }

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

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        switch (message)
        {
            case DeviceMessage.VentOpenChanged openChanged:
                IsOpen = openChanged.IsOpen;
                OpenButtonColour = new SolidColorBrush(IsOpen
                    ? Color.FromRgb(0,255,0)
                    : Color.FromRgb(255,0,0));
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
    }
}
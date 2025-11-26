using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class LightControllerViewModel : DeviceViewModel, IOnOff
{
    public LightControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus)
        : base(deviceController, messageBus) { }

    [ObservableProperty]
    private bool _isOn;

    [ObservableProperty]
    private SolidColorBrush? _buttonColour;

    [ObservableProperty]
    private double _dimmer;

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        switch (message)
        {
            case DeviceMessage.DeviceOnOffChanged onOffChanged:
                IsOn = onOffChanged.IsOn;
                ButtonColour = new SolidColorBrush(IsOn
                    ? Color.FromRgb(0,255,0)
                    : Color.FromRgb(255,0,0));
                break;
            case DeviceMessage.LightDimmerLevelChanged dimmerLevelChanged:
                Dimmer = dimmerLevelChanged.DimmerLevel * 100;
                break;
        }
    }
}
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
    private string? _buttonOnText;

    [ObservableProperty]
    private double _dimmer;

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        switch (message)
        {
            case DeviceMessage.DeviceOnOffChanged:
                IsOn = (bool)message.Data!;
                ButtonOnText = IsOn
                    ? "Turn Light Off"
                    : "Turn Light On";
                break;
            case DeviceMessage.DimmerLevelChanged:
                Dimmer = (double)message.Data! * 100;
                break;
        }
    }
}
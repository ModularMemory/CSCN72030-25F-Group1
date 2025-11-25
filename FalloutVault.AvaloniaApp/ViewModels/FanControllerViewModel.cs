using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class FanControllerViewModel : DeviceViewModel, IOnOff
{
    public FanControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus)
        : base(deviceController, messageBus) { }

    [ObservableProperty]
    private bool _isOn;

    [ObservableProperty]
    private int _currentSpeed;

    [ObservableProperty]
    private int _targetSpeed;

    [ObservableProperty]
    private SolidColorBrush? _buttonColour;

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        switch (message)
        {
            case DeviceMessage.DeviceOnOffChanged:
                IsOn = (bool)message.Data!;
                ButtonColour = new SolidColorBrush(IsOn
                    ? Color.FromRgb(0,255,0)
                    : Color.FromRgb(255,0,0));
                break;
            case DeviceMessage.FanSpeedRpmChanged:
                CurrentSpeed = (int)message.Data!;
                break;
            case DeviceMessage.FanTargetRpmChanged:
                TargetSpeed = (int)message.Data!;
                break;
        }
    }
}
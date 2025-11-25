using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class SpeakerControllerViewModel : DeviceViewModel, IOnOff
{
    public SpeakerControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus)
        : base(deviceController, messageBus) { }

    [ObservableProperty]
    private bool _isOn;

    [ObservableProperty]
    private string? _buttonOnText;

    [ObservableProperty]
    private double _volume;

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        switch (message)
        {
            case DeviceMessage.DeviceOnOffChanged:
                IsOn = (bool)message.Data!;
                ButtonOnText = IsOn
                    ? "Turn Speaker Off"
                    : "Turn Speaker On";
                break;
            case DeviceMessage.VolumeLevelChanged:
                Volume = (double)message.Data! * 100;
                break;
        }
    }
}
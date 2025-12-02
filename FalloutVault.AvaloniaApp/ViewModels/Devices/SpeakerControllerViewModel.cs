using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public partial class SpeakerControllerViewModel : DeviceViewModel, IOnOff
{
    public SpeakerControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus)
        : base(deviceController, messageBus) { }

    [ObservableProperty]
    public partial bool IsOn { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? ButtonColour { get; set; }

    [ObservableProperty]
    public partial double Volume { get; set; }

    partial void OnVolumeChanged(double value)
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetSpeakerVolume(value / 100));
    }

    [RelayCommand]
    public void OnOffButton_OnClick()
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetOn(!IsOn));
    }

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
            case DeviceMessage.VolumeLevelChanged volumeLevelChanged:
                Volume = volumeLevelChanged.Volume * 100;
                break;
        }
    }
}
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public partial class CropSprinklerControllerViewModel : PoweredDeviceViewModel, IOnOff
{
    public CropSprinklerControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus,
        IEventBus<Watt> powerBus,
        ILogger logger)
        : base(deviceController, messageBus, powerBus, logger) { }

    [ObservableProperty]
    public partial bool IsOn { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? ButtonColour { get; set; }

    [ObservableProperty]
    public partial Watt PowerDraw { get; set; }

    [RelayCommand]
    public void OnOffButton_OnClick()
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetOn(!IsOn));
    }

    protected override void OnDeviceMessage(object? sender, DeviceMessage message)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        Dispatcher.UIThread.Invoke(() =>
        {
            switch (message)
            {
                case DeviceMessage.DeviceOnOffChanged onOffChanged:
                    IsOn = onOffChanged.IsOn;
                    ButtonColour = new SolidColorBrush(IsOn
                        ? Color.FromRgb(0, 255, 0)
                        : Color.FromRgb(255, 0, 0));
                    break;
            }
        });
    }

    protected override void OnPowerMessage(object? sender, Watt watts)
    {
        if (sender is not IDevice device || device.Id != Id)
            return;

        PowerDraw = watts;
    }
}
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.Views;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public partial class FanControllerViewModel : DeviceViewModel, IOnOff
{
    public FanControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus,
        ILogger logger)
        : base(deviceController, messageBus, logger) { }

    [ObservableProperty]
    public partial bool IsOn { get; set; }

    [ObservableProperty]
    public partial int CurrentSpeed { get; set; }

    [ObservableProperty]
    public partial int TargetSpeed { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? ButtonColour { get; set; }

    partial void OnTargetSpeedChanged(int value)
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetFanTargetRpm(value));
    }
    [RelayCommand]
    public async void TimedButton_OnClick()
    {
        var dialog = new TimedOnOffDialog
        {
            DataContext = new TimedOnOffDialogViewModel(),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        TimedOnOffDialogViewModel? dialogViewModel = null;
        try
        {
            var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            dialogViewModel = await dialog.ShowDialog<TimedOnOffDialogViewModel>(appLifetime?.MainWindow!);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error while showing timed on off dialog.");
        }

        if (dialogViewModel is null)
        {
            return;
        }

        var result = dialogViewModel.Result;
        var duration = dialogViewModel.Duration;
        DeviceCommand command = result switch
        {
            TimedOnOffDialogResult.On => new DeviceCommand.TurnOnFor(duration),
            TimedOnOffDialogResult.Off => new DeviceCommand.TurnOffFor(duration),
            _ => throw new UnreachableException()
        };

        DeviceController.SendCommand(Id, command);
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
                case DeviceMessage.FanSpeedRpmChanged speedRpmChanged:
                    CurrentSpeed = speedRpmChanged.SpeedRpm;
                    break;
                case DeviceMessage.FanTargetRpmChanged targetRpmChanged:
                    TargetSpeed = targetRpmChanged.TargetRpm;
                    break;
            }
        });
    }
}
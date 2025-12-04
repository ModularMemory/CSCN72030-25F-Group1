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
using FalloutVault.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public partial class FanControllerViewModel : PoweredDeviceViewModel, IOnOff
{
    public FanControllerViewModel(
        IDeviceController deviceController,
        IEventBus<DeviceMessage> messageBus,
        IEventBus<Watt> powerBus,
        ILogger logger)
        : base(deviceController, messageBus, powerBus, logger) { }

    [ObservableProperty]
    public partial bool IsOn { get; set; }

    [ObservableProperty]
    public partial int CurrentSpeed { get; set; }

    [ObservableProperty]
    public partial int TargetSpeed { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? ButtonColour { get; set; }

    [ObservableProperty]
    public partial Watt MotorWattage { get; set; }

    [ObservableProperty]
    public partial int MaxRpm { get; set; } = int.MaxValue;

    [ObservableProperty]
    public partial Watt PowerDraw { get; set; }

    [ObservableProperty]
    public partial TimeSpan? TimedOnOffRemaining { get; set; }

    [ObservableProperty]
    public partial int IconSize { get; set; } = 20;

    partial void OnTargetSpeedChanged(int value)
    {
        DeviceController.SendCommand(Id, new DeviceCommand.SetFanTargetRpm(value));
    }

    [RelayCommand]
    public async void TimedButton_OnClick()
    {
        var dialog = new TimedOnOffDialog(IsOn)
        {
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
                case DeviceMessage.FanMotorWattage motorWattage:
                    MotorWattage = motorWattage.Wattage;
                    break;
                case DeviceMessage.FanMaxRpm maxRpm:
                    MaxRpm = maxRpm.MaxRpm;
                    break;
                case DeviceMessage.DeviceTimedOnOffChanged timedOnOffChanged:
                    TimedOnOffRemaining =
                        timedOnOffChanged.TimeRemaining > TimeSpan.Zero
                            ? timedOnOffChanged.TimeRemaining
                            : null;
                    IconSize =
                        timedOnOffChanged.TimeRemaining > TimeSpan.Zero
                            ? 14
                            : 20;
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
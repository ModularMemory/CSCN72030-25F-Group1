using Avalonia.Controls;
using Avalonia.Interactivity;
using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.AvaloniaApp.ViewModels.Devices;
using FalloutVault.Devices.Interfaces;

namespace FalloutVault.AvaloniaApp.Views;

public partial class TimedOnOffDialog : Window
{
    private readonly bool _sourceIsOn;

    public TimedOnOffDialog(bool sourceIsOn)
    {
        _sourceIsOn = sourceIsOn;

        InitializeComponent();
    }

    private void AcceptButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var vm = DataContext as TimedOnOffDialogViewModel;

        vm?.Result = _sourceIsOn switch
        {
            true => TimedOnOffDialogResult.Off,
            false => TimedOnOffDialogResult.On
        };

        Close(DataContext);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
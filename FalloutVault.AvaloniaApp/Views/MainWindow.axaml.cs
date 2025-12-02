using Avalonia.Controls;
using Avalonia.Interactivity;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.AvaloniaApp.ViewModels.Devices;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Interfaces;
using Serilog;

namespace FalloutVault.AvaloniaApp.Views;

public partial class MainWindow : Window
{
    private readonly IDeviceController _deviceController;
    private readonly ILogger _logger;

    public MainWindow(IDeviceController deviceController, ILogger logger)
    {
        _deviceController = deviceController;
        _logger = logger;

        InitializeComponent();
    }

    private void TimedButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!IsInitialized || sender is not Button { DataContext: IDeviceViewModel viewModel })
            return;

        if (viewModel is not IOnOff onOffDevice)
        {
            _logger.Warning("Tried to turn off a device ({DeviceType}) that does not support IOnOff", viewModel.GetType());
            return;
        }

        _deviceController.SendCommand(viewModel.Id, new DeviceCommand.SetOn(!onOffDevice.IsOn));
    }

    private void CheckAllZones_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (!IsInitialized || sender is not CheckBox checkBox || DataContext is not MainWindowViewModel viewModel)
            return;

        foreach (var zoneViewModel in viewModel.Zones)
        {
            zoneViewModel.IsSelected = checkBox.IsChecked.GetValueOrDefault();
        }
    }

    private async void CheckZone_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (!IsInitialized || sender is not CheckBox checkBox || DataContext is not MainWindowViewModel viewModel)
            return;

        await Task.Yield();

        viewModel.UpdateDeviceList();
    }
}
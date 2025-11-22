using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using FalloutVault.AvaloniaApp.ViewModels;
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

    private void SliderDimmer_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is not Slider { DataContext: IDeviceViewModel viewModel })
            return;

        _deviceController.SendCommand(viewModel.Id, new DeviceCommand.SetLightDimmer(e.NewValue/100));
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: IDeviceViewModel viewModel })
            return;

        if (viewModel is not IOnOff onOffDevice)
        {
            _logger.Warning("Tried to turn off a device ({DeviceType}) that does not support IOnOff", viewModel.GetType());
            return;
        }

        _deviceController.SendCommand(viewModel.Id, new DeviceCommand.SetOn(!onOffDevice.IsOn));
    }
}
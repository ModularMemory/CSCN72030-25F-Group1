using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using FalloutVault.AvaloniaApp.ViewModels;
using FalloutVault.Commands;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.Views;

public partial class MainWindow : Window
{
    private readonly IDeviceController _deviceController;

    public MainWindow(IDeviceController deviceController)
    {
        _deviceController = deviceController;

        InitializeComponent();
    }

    private void SliderDimmer_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        var viewModel = (IDeviceViewModel)((Slider)sender).DataContext;

        _deviceController.SendCommand(viewModel.Id, new DeviceCommand.SetLightDimmer(e.NewValue/100));
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (IDeviceViewModel)((Button)sender).DataContext;

        _deviceController.SendCommand(viewModel.Id, new DeviceCommand.SetOn());
        throw new NotImplementedException();
    }
}
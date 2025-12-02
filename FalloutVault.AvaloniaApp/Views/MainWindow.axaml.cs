using Avalonia.Controls;
using Avalonia.Interactivity;
using FalloutVault.AvaloniaApp.ViewModels;
using Serilog;

namespace FalloutVault.AvaloniaApp.Views;

public partial class MainWindow : Window
{
    private readonly ILogger _logger;

    public MainWindow(ILogger logger)
    {
        _logger = logger;

        InitializeComponent();
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
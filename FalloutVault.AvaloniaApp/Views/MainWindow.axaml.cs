using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.Services.Interfaces;
using FalloutVault.AvaloniaApp.ViewModels;
using Serilog;

namespace FalloutVault.AvaloniaApp.Views;

public partial class MainWindow : Window
{
    private readonly ILogger _logger;

    public MainWindow(IDeviceMessageLogger deviceMessageLogger, ILogger logger)
    {
        _logger = logger;
        deviceMessageLogger.DeviceMessageReceived += DeviceMessageLoggerOnDeviceMessageReceived;

        InitializeComponent();
    }

    private void DeviceMessageLoggerOnDeviceMessageReceived(object? sender, DeviceLog e)
    {
        Dispatcher.UIThread.InvokeAsync(Callback);
        return;

        async Task Callback()
        {
            await Task.Yield(); // Try to run after any other event subscribes
            var lastItem = ((IEnumerable<object>?)LogDataGrid.ItemsSource)?.LastOrDefault();
            LogDataGrid.ScrollIntoView(lastItem, LogDataGrid.Columns.FirstOrDefault());
        }
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

    private void CheckAllTypes_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (!IsInitialized || sender is not CheckBox checkBox || DataContext is not MainWindowViewModel viewModel)
            return;

        foreach (var deviceNavigationViewModel in viewModel.Types)
        {
            deviceNavigationViewModel.IsSelected = checkBox.IsChecked.GetValueOrDefault();
        }
    }

    private async void CheckType_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (!IsInitialized || sender is not CheckBox checkBox || DataContext is not MainWindowViewModel viewModel)
            return;

        await Task.Yield();

        viewModel.UpdateDeviceList();
    }
}
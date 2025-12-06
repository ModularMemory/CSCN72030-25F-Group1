using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
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

    private void LogDataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (e.Source is not Control { DataContext: LogViewModel logViewModel })
            return;

        if (DataContext is not MainWindowViewModel vm)
            return;

        var senderVm = vm.Devices.FirstOrDefault(x => x.Id == logViewModel.Sender);
        ItemsDevices.ScrollIntoView(senderVm!);
    }

    private async void OnDataContextChanged(object? sender, EventArgs e)
    {
        await Task.Yield();
        if (DataContext is not MainWindowViewModel vm)
            return;

        vm.LogMessages.CollectionChanged += LogMessages_OnCollectionChanged;
        LogMessages_OnCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void LogMessages_OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            var lastItem = ((IEnumerable<object>?)LogDataGrid.ItemsSource)?.LastOrDefault();
            LogDataGrid.ScrollIntoView(lastItem, LogDataGrid.Columns.FirstOrDefault());
        });
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
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.Services.Interfaces;
using FalloutVault.AvaloniaApp.ViewModels.Devices;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDeviceMessageLogger _deviceMessageLogger;
    private readonly List<IDeviceViewModel> _deviceViewModels = [];
    private readonly Lock _logLock = new();

    public ObservableCollection<ZoneViewModel> Zones { get; } = [];
    public ObservableCollection<DeviceNavigationViewModel> Types { get; } = [];
    public ObservableCollection<IDeviceViewModel> Devices { get; } = [];
    public ObservableCollection<LogViewModel> LogMessages { get; } = [];

    [ObservableProperty]
    public partial string LogSearch { get; set; }

    [ObservableProperty]
    public partial PowerBarViewModel PowerBar { get; set; }

    [ObservableProperty]
    public partial bool OrderByDevice { get; set; } = true;

    [ObservableProperty]
    public partial bool OrderByZone { get; set; }

    partial void OnOrderByDeviceChanged(bool value)
    {
        if (value)
        {
            OrderByZone = false;
            UpdateDeviceList();
        }
    }

    partial void OnOrderByZoneChanged(bool value)
    {
        if (value)
        {
            OrderByDevice = false;
            UpdateDeviceList();
        }
    }

    public MainWindowViewModel(
        IDeviceRegistry deviceRegistry,
        IDeviceMessageLogger deviceMessageLogger,
        DeviceViewModelFactory deviceViewModelFactory,
        PowerBarViewModel powerBarViewModel)
    {
        PowerBar = powerBarViewModel;
        _deviceMessageLogger = deviceMessageLogger;

        lock (_logLock)
        {
            if (_deviceMessageLogger.Messages.Count > 0)
            {
                foreach (var message in _deviceMessageLogger.Messages)
                {
                    AddLogMessage(message);
                }
            }
        }

        _deviceMessageLogger.DeviceMessageReceived += DeviceMessageLogger_OnDeviceMessageReceived;

        foreach (var (id, type, capabilities) in deviceRegistry.Devices)
        {
            _deviceViewModels.Add(deviceViewModelFactory.Create(type, id));
        }

        foreach (var zone in deviceRegistry.Devices.Select(x => x.id.Zone).Distinct().Order())
        {
            Zones.Add(new ZoneViewModel(zone));
        }

        foreach (var type in deviceRegistry.Devices.Select(x => x.type).Distinct().Order())
        {
            Types.Add(new DeviceNavigationViewModel(type));
        }

        UpdateDeviceList();
    }

    public void UpdateDeviceList()
    {
        var enabledZones = Zones
            .Where(x => x.IsSelected)
            .Select(x => x.ZoneName)
            .ToHashSet();

        var enabledTypes = Types
            .Where(x => x.IsSelected)
            .Select(x => x.Type)
            .ToHashSet();


        var devices =
            OrderByDevice
                ? _deviceViewModels
                    .OrderBy(x => x.Type)
                    .ThenBy(x => x.Id.Zone)
                : _deviceViewModels
                    .OrderBy(x => x.Id.Zone)
                    .ThenBy(x => x.Type);

        Devices.Clear();
        foreach (var viewModel in devices)
        {
            if (enabledZones.Contains(viewModel.Id.Zone) && enabledTypes.Contains(viewModel.Type))
            {
                Devices.Add(viewModel);
            }
        }
    }

    private void DeviceMessageLogger_OnDeviceMessageReceived(object? sender, DeviceLog e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            lock (_logLock)
            {
                AddLogMessage(e);
            }
        });
    }

    partial void OnLogSearchChanged(string value)
    {
        lock (_logLock)
        {
            LogMessages.Clear();
            foreach (var deviceLog in _deviceMessageLogger.Messages)
            {
                AddLogMessage(deviceLog);
            }
        }
    }

    private void AddLogMessage(DeviceLog log)
    {
        if (string.IsNullOrWhiteSpace(LogSearch)
            || log.Sender.Name.Contains(LogSearch, StringComparison.OrdinalIgnoreCase)
            || log.Sender.Zone.Contains(LogSearch, StringComparison.OrdinalIgnoreCase)
            || log.Message.Message.Contains(LogSearch, StringComparison.OrdinalIgnoreCase)
            || (log.DataString != null && log.DataString.Contains(LogSearch, StringComparison.OrdinalIgnoreCase)))
        {
            LogMessages.Add(new LogViewModel(log));
        }

        if (LogMessages.Count > 500)
        {
            LogMessages.RemoveAt(0);
        }
    }
}

// Source - https://stackoverflow.com/a
// Posted by kekekeks
// Retrieved 2025-11-20, License - CC BY-SA 4.0
public class MyTemplateSelector : IDataTemplate
{
    // ReSharper disable once CollectionNeverUpdated.Global
    [Content]
    public Dictionary<DeviceType, IDataTemplate> Templates { get; } = [];

    public Control? Build(object? data)
    {
        return Templates[((DeviceViewModel)data!).Type].Build(data);
    }

    public bool Match(object? data)
    {
        return data is DeviceViewModel;
    }
}
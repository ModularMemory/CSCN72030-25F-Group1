using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using FalloutVault.AvaloniaApp.Models;
using FalloutVault.AvaloniaApp.Services.Interfaces;
using FalloutVault.AvaloniaApp.ViewModels.Devices;
using FalloutVault.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IDeviceMessageLogger _deviceMessageLogger;
    private readonly List<IDeviceViewModel> _deviceViewModels = [];

    public ObservableCollection<ZoneViewModel> Zones { get; } = [];
    public ObservableCollection<IDeviceViewModel> Devices { get; } = [];
    public ObservableCollection<LogViewModel> LogMessages { get; } = [];

    public MainWindowViewModel(IDeviceRegistry deviceRegistry, IDeviceMessageLogger deviceMessageLogger, DeviceViewModelFactory deviceViewModelFactory)
    {
        _deviceMessageLogger = deviceMessageLogger;
        _deviceMessageLogger.DeviceMessageReceived += DeviceMessageLogger_OnDeviceMessageReceived;

        foreach (var message in _deviceMessageLogger.Messages)
        {
            DeviceMessageLogger_OnDeviceMessageReceived(null, message);
        }

        foreach (var (id, type, capabilities) in deviceRegistry.Devices)
        {
            _deviceViewModels.Add(deviceViewModelFactory.Create(type, id));
        }

        foreach (var zone in deviceRegistry.Devices.Select(x => x.id.Zone).Distinct())
        {
            Zones.Add(new ZoneViewModel(zone));
        }

        UpdateDeviceList();
    }

    public void UpdateDeviceList()
    {
        var enabledZones = Zones
            .Where(x => x.IsSelected)
            .Select(x => x.ZoneName)
            .ToHashSet();

        Devices.Clear();
        foreach (var viewModel in _deviceViewModels)
        {
            if (enabledZones.Contains(viewModel.Id.Zone))
            {
                Devices.Add(viewModel);
            }
        }
    }

    private void DeviceMessageLogger_OnDeviceMessageReceived(object? sender, DeviceLog e)
    {
        LogMessages.Add(new LogViewModel(e.Sender, e.Message));
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
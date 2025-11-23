using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using FalloutVault.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string LeftTitle { get; } = "Rooms";
    public string RightTitle { get; } = "Devices";

    public ObservableCollection<IDeviceViewModel> Devices { get; } = [];

    public MainWindowViewModel(IDeviceRegistry deviceRegistry, DeviceViewModelFactory deviceViewModelFactory)
    {
        foreach (var (id, type, capabilities) in deviceRegistry.Devices)
        {
            Devices.Add(deviceViewModelFactory.Create(type, id));
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

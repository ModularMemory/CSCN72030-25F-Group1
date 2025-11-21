using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using FalloutVault.Devices.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string LeftTitle { get; } = "Rooms";
    public string RightTitle { get; } = "Devices";

    public ObservableCollection<MyModel> Devices { get; } = new();

    public MainWindowViewModel(IDeviceRegistry deviceRegistry)
    {
        foreach (var (id, type, capabilities) in deviceRegistry.Devices)
        {
            Devices.Add(new MyModel(id, type));
        }

    }

}

// Source - https://stackoverflow.com/a
// Posted by kekekeks
// Retrieved 2025-11-20, License - CC BY-SA 4.0

public class MyTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<DeviceType, IDataTemplate> Templates {get;} = new();

    public Control Build(object? data)
    {
        return Templates[((MyModel) data).Value].Build(data);
    }

    public bool Match(object? data)
    {
        return data is MyModel;
    }
}

public class MyModel
{
    public MyModel(DeviceId id, DeviceType text)
    {
        Id = id;
        Value = text;
    }
    public DeviceId Id { get; }
    public DeviceType Value { get; }
}
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Models;
using Humanizer;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class DeviceNavigationViewModel : ViewModelBase
{
    public DeviceNavigationViewModel(DeviceType type)
    {
        Type = type;
        Name = type.Humanize(LetterCasing.Title);
        IsSelected = true;
    }

    public DeviceType Type { get; }

    public string Name { get; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}
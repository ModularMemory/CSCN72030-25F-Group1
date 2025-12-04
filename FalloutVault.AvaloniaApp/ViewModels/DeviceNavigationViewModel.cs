using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class DeviceNavigationViewModel : ViewModelBase
{
    public DeviceNavigationViewModel(DeviceType type)
    {
        Type = type;
        IsSelected = true;
    }

    public DeviceType Type { get; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    [ObservableProperty]
    public partial bool AllZonesSelected { get; set; } = true;
}
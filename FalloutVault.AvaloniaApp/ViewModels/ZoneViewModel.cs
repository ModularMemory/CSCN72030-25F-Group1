using CommunityToolkit.Mvvm.ComponentModel;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class ZoneViewModel : ViewModelBase
{
    public ZoneViewModel(string zoneName)
    {
        ZoneName = zoneName;
        IsSelected = true;
    }

    public string ZoneName { get; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; }

    [ObservableProperty]
    public partial bool AllZonesSelected { get; set; } = true;
}
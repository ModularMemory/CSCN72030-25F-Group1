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
    private bool _isSelected;

    [ObservableProperty]
    private bool _allZonesSelected = true;
}
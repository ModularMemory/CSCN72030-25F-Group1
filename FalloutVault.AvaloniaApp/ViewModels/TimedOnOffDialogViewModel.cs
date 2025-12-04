using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.AvaloniaApp.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class TimedOnOffDialogViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial TimedOnOffDialogResult Result { get; set; }

    [ObservableProperty]
    public partial TimeSpan Duration { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; } = "Timed Device On/Off";
}
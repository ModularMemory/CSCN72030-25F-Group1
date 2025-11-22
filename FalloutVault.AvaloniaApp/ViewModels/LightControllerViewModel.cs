using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using CommunityToolkit.Mvvm.ComponentModel;
using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.ViewModels;

public partial class LightControllerViewModel : DeviceViewModel
{
    private readonly IDeviceController _deviceController;
    private readonly Binding _dimmerBinding;
    public LightControllerViewModel(IDeviceController deviceController)
    {
        _deviceController = deviceController;
    }
    private void SliderDimmer_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    [ObservableProperty]
    private bool _isLightOn;

    [ObservableProperty]
    private double _dimmer;
}
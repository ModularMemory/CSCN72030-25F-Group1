using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;


public abstract class DeviceViewModel : ViewModelBase, IDeviceViewModel
{

    public DeviceId Id
    {
        get;
        set
        {
            if (field != default) throw new InvalidOperationException();

            field = value;
        }
    }

    public DeviceType Type
    {
        get;
        set
        {
            if (field != DeviceType.Unknown) throw new InvalidOperationException();

            field = value;
        }
    }
}
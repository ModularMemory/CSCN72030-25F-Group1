using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class DeviceViewModelFactory
{
    private readonly Dictionary<DeviceType, Func<IDeviceViewModel>> _dict;

    public DeviceViewModelFactory(Dictionary<DeviceType, Func<IDeviceViewModel>> dict)
    {
        _dict = dict;
    }

    public IDeviceViewModel Create(DeviceType deviceType, DeviceId deviceId)
    {
        var device = _dict[deviceType].Invoke();
        device.Id = deviceId;
        device.Type = deviceType;
        return device;
    }
}
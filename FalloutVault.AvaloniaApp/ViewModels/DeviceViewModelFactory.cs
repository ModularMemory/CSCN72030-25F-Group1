using FalloutVault.Devices.Models;
using FalloutVault.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class DeviceViewModelFactory
{
    private readonly Dictionary<DeviceType, Func<IDeviceViewModel>> _constructors = [];

    public DeviceViewModelFactory(IServiceProvider serviceProvider)
    {
        var viewModels = Enum.GetValues<DeviceType>()
            .Where(x => x != DeviceType.Unknown);

        foreach (var deviceType in viewModels)
        {
            _constructors.Add(deviceType, () => serviceProvider.GetRequiredKeyedService<IDeviceViewModel>(deviceType));
        }
    }

    public IDeviceViewModel Create(DeviceType deviceType, DeviceId deviceId)
    {
        var device = _constructors[deviceType].Invoke();

        device.Id = deviceId;
        device.Type = deviceType;
        device.ForceUpdateCurrentState();

        return device;
    }
}
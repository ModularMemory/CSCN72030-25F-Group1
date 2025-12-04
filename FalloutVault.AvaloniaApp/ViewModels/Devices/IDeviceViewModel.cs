using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public interface IDeviceViewModel
{
    DeviceType Type { get; set; }
    DeviceId Id { get; set; }
    void ForceUpdateCurrentState();


}
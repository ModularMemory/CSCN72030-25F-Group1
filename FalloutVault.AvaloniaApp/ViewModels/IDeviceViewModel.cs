using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public interface IDeviceViewModel
{
    DeviceType Type { get; set; }
    DeviceId Id { get; set; }
}
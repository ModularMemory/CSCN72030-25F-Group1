using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;

namespace FalloutVault.AvaloniaApp.Models;

public record DeviceLog(DeviceId Sender, DeviceMessage Message);
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IDevice : IEventBusMember<DeviceMessage>, IEventBusMember<WattHours>
{
    /// <summary>
    /// The name of the device.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The zone where the device is located.
    /// </summary>
    string Zone { get; }

    /// <summary>
    /// Triggers a manual update check.
    /// </summary>
    void Update();
}
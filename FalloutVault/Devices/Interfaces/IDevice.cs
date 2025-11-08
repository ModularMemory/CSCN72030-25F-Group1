using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IDevice : IEventBusMember<DeviceMessage>, IEventBusMember<Watt>
{
    /// <summary>
    /// The ID of the device.
    /// </summary>
    DeviceId Id { get; }

    /// <summary>
    /// The current power draw of the device.
    /// </summary>
    Watt PowerDraw { get; }

    /// <summary>
    /// Triggers a manual update check.
    /// </summary>
    void Update();
}
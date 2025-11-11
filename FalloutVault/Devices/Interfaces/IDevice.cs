using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IDevice : IEventBusMember<DeviceMessage>, IEventBusMember<Watt>
{
    /// <summary>
    /// The ID of the device.
    /// </summary>
    DeviceId Id { get; }

    /// <summary>
    /// Triggers a manual update check.
    /// </summary>
    void Update();

    /// <summary>
    /// Sends a command to the device.
    /// </summary>
    /// <param name="command">The command to send.</param>
    void SendCommand(DeviceCommand command);
}
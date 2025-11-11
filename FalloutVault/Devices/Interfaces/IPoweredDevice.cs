using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IPoweredDevice
{
    /// <summary>
    /// The current power draw of the device.
    /// </summary>
    Watt PowerDraw { get; }
}
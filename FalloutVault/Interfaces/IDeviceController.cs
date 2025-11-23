using FalloutVault.Commands;
using FalloutVault.Devices.Models;

namespace FalloutVault.Interfaces;

public interface IDeviceController
{
    /// <summary>
    /// Starts the controller with the default polling interval of 50ms.
    /// </summary>
    void Start() => Start(TimeSpan.FromMilliseconds(50));

    /// <summary>
    /// Starts the controller with a custom polling interval.
    /// </summary>
    void Start(TimeSpan pollingInterval);

    /// <summary>
    /// Stops the controller.
    /// </summary>
    void Stop();

    /// <summary>
    /// Sends a command to the specified device.
    /// </summary>
    /// <param name="targetDevice">The device to send the command to.</param>
    /// <param name="command">The command to send.</param>
    /// <returns>True if the command was successfully sent to the device, otherwise false.</returns>
    bool SendCommand(DeviceId targetDevice, DeviceCommand command);

    /// <summary>
    /// Sends a command to all devices in specified zone.
    /// </summary>
    /// <param name="zone">The zone to search for devices to send to. Case-insensitive.</param>
    /// <param name="command">The command to send.</param>
    /// <returns>True if the command was successfully sent to one or more devices, otherwise false.</returns>
    bool SendCommandToZone(string zone, DeviceCommand command);
}
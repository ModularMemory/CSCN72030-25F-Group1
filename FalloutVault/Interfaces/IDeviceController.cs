using FalloutVault.Devices.Interfaces;

namespace FalloutVault.Interfaces;

public interface IDeviceController
{
    /// <summary>
    /// Registers a new device with the controller and the related registry.
    /// </summary>
    /// <param name="device">The device to register.</param>
    /// <typeparam name="TDevice">A class that implements the <see cref="IDevice"/> interface.</typeparam>
    /// <returns>The controller instance.</returns>
    /// <exception cref="ArgumentException">A <paramref name="device"/> with the same name and zone was already added.</exception>
    IDeviceController AddDevice<TDevice>(TDevice device) where TDevice : IDevice;

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
}
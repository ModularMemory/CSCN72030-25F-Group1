using FalloutVault.Interfaces;

namespace FalloutVault.AvaloniaApp.Services.Interfaces;

public interface IDeviceFactory
{
    /// <summary>
    /// Adds the devices known by the factory to the <see cref="IDeviceController"/>.
    /// </summary>
    void AddDevices();

    /// <summary>
    /// Initializes the devices added to the <see cref="IDeviceController"/> by this <see cref="IDeviceFactory"/>.
    /// </summary>
    /// <remarks>
    /// Must be called after UI initialization to avoid desynchronization issues.
    /// </remarks>
    void InitializeDevices();
}
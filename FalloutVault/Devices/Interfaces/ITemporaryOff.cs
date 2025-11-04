namespace FalloutVault.Devices.Interfaces;

public interface ITemporaryOff
{
    /// <summary>
    /// Temporarily disables a device. The device will be re-enabled after <paramref name="time"/>.
    /// </summary>
    /// <param name="time">The time to turn off for.</param>
    void TurnOffFor(TimeSpan time);
}
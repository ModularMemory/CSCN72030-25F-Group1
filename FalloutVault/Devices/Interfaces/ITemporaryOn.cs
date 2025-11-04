namespace FalloutVault.Devices.Interfaces;

public interface ITemporaryOn
{
    /// <summary>
    /// Temporarily enables a device. The device will be disabled after <paramref name="time"/>.
    /// </summary>
    /// <param name="time">The time to turn on for.</param>
    void TurnOnFor(TimeSpan time);
}
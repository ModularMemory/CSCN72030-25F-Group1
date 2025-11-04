namespace FalloutVault.Devices.Interfaces;

public interface IPeriodic
{
    /// <summary>
    /// The time between periodic events.
    /// </summary>
    TimeSpan Interval { get; }

    /// <summary>
    /// How long each periodic event will last.
    /// </summary>
    TimeSpan Duration { get; }
}
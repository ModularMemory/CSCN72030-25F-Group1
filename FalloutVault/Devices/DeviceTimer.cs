using System.Diagnostics;

namespace FalloutVault.Devices;

public sealed class DeviceTimer<T>
{
    public bool IsRunning { get; private set; }
    public T? State { get; private set; }

    private readonly Stopwatch _stopwatch = new();
    private TimeSpan _duration;

    /// <summary>
    /// Checks if the timer has expired and updates <see cref="IsRunning"/> accordingly.
    /// </summary>
    public void Update()
    {
        if (!IsRunning) return;

        if (_stopwatch.Elapsed >= _duration)
        {
            IsRunning = false;
        }
    }

    /// <summary>
    /// Sets the timer and an optional state to be referenced after the timer ends.
    /// </summary>
    /// <param name="time">The minimum time the timer should run for.</param>
    /// <param name="state">An optional state for the caller to reference after the timer completes.</param>
    public void SetTimer(TimeSpan time, T? state)
    {
        IsRunning = true;
        State = state;
        _stopwatch.Restart();
        _duration = time;
    }
}
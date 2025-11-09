using System.Diagnostics;

namespace FalloutVault.Devices;

/// <summary>
/// A thread-unsafe timer which must be polled manually to check for completion.
/// </summary>
/// <typeparam name="TState">The type of the optional state property.</typeparam>
public sealed class DeviceTimer<TState>
{
    public TState? State { get; private set; }

    private readonly Stopwatch _stopwatch = new();
    private bool _isRunning;
    private TimeSpan _duration;

    /// <summary>
    /// Checks if the timer has expired.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the timer completed on this check, otherwise <see langword="false"/>.
    /// </returns>
    public bool CheckCompleted()
    {
        if (!_isRunning) return false;

        if (_stopwatch.Elapsed < _duration) return false;

        _isRunning = false;
        return true;

    }

    /// <summary>
    /// Sets the timer and an optional state to be referenced after the timer ends.
    /// </summary>
    /// <param name="time">The minimum time the timer should run for.</param>
    /// <param name="state">An optional state for the caller to reference after the timer completes.</param>
    public void SetTimer(TimeSpan time, TState? state)
    {
        _isRunning = true;
        State = state;
        _stopwatch.Restart();
        _duration = time;
    }
}
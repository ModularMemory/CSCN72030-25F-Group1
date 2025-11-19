namespace FalloutVault.Eventing.Models;

public abstract partial class DeviceMessage(object? data, DateTimeOffset timestamp)
{
    public DateTimeOffset Timestamp { get; } = timestamp;
    public object? Data { get; } = data;
    public abstract string Message { get; }

    protected DeviceMessage(object? data) : this(data, DateTimeOffset.UtcNow) { }
}
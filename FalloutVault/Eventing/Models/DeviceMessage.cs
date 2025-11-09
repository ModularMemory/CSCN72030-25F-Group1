namespace FalloutVault.Eventing.Models;

public abstract partial class DeviceMessage(DateTimeOffset timestamp, object? data)
{
    public DateTimeOffset Timestamp { get; } = timestamp;
    public object? Data { get; } = data;
    public abstract string Message { get; }

    protected DeviceMessage(object? data) : this(DateTimeOffset.UtcNow, data) { }
}
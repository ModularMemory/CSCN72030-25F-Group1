namespace FalloutVault.Eventing.Models;

public abstract partial class DeviceMessage(object? data)
{
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    public object? Data { get; } = data;
    public abstract string Message { get; }
}
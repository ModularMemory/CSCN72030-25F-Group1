namespace FalloutVault.Eventing.Models;

public sealed record DeviceMessage(DateTimeOffset Timestamp, string Message, object? Data)
{
    public DeviceMessage(string message, object? data) : this(DateTimeOffset.UtcNow, message, data) { }
}
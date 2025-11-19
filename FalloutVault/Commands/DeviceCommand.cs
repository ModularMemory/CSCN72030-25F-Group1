namespace FalloutVault.Commands;

public abstract partial class DeviceCommand(object? data, DateTimeOffset timestamp)
{
    public DateTimeOffset Timestamp { get; } = timestamp;
    public object? Data { get; } = data;

    protected DeviceCommand(object? data) : this(data, DateTimeOffset.UtcNow) { }
}
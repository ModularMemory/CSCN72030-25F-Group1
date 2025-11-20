namespace FalloutVault.Commands;

public abstract partial class DeviceCommand(object? data)
{
    public object? Data { get; } = data;
}
using FalloutVault.Devices.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    public abstract string Name { get; }
    public abstract string Zone { get; }
    public abstract EventHandler<DeviceMessage>? OnDeviceMessage { get; }
    public abstract void Update();

    protected void SendDeviceMessage(DeviceMessage message)
    {
        OnDeviceMessage?.Invoke(this, message);
    }
}
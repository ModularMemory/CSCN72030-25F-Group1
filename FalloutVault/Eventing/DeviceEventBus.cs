using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Eventing;

public sealed class DeviceEventBus : IEventBus<DeviceMessage>
{
    public event EventHandler<DeviceMessage>? Handler;

    public void Publish(object sender, DeviceMessage data)
    {
        Handler?.Invoke(sender, data);
    }
}
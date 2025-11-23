using FalloutVault.Eventing.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.Eventing;

public sealed class PowerEventBus : IEventBus<Watt>
{
    public event EventHandler<Watt>? Handler;

    public void Publish(object sender, Watt data)
    {
        Handler?.Invoke(sender, data);
    }
}
using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    public abstract string Name { get; }
    public abstract string Zone { get; }
    protected IEventBus<DeviceMessage>? MessageBus { get; private set; }
    protected IEventBus<WattHours>? PowerBus { get; private set; }

    public abstract void Update();

    public virtual void SetEventBus(IEventBus<DeviceMessage> eventBus) => MessageBus = eventBus;

    public virtual void SetEventBus(IEventBus<WattHours> eventBus) => PowerBus = eventBus;
}
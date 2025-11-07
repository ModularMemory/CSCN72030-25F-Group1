using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    // IDevice members
    public abstract string Name { get; }
    public abstract string Zone { get; }
    public abstract void Update();

    // Message bus
    protected IEventBus<DeviceMessage>? MessageBus { get; private set; }
    public virtual void SetEventBus(IEventBus<DeviceMessage> eventBus) => MessageBus = eventBus;
    protected void PublishMessage(DeviceMessage message) => MessageBus?.Publish(this, message);

    // Power bus
    protected IEventBus<WattHours>? PowerBus { get; private set; }
    public virtual void SetEventBus(IEventBus<WattHours> eventBus) => PowerBus = eventBus;
    protected void PublishPowerUsage(WattHours power) => PowerBus?.Publish(this, power);
}
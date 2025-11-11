using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    // IDevice members
    public abstract DeviceId Id { get; }

    public Watt PowerDraw
    {
        get;
        protected set
        {
            if (!SetField(ref field, value)) return;

            PublishPowerUsage(field);
        }
    }

    public abstract void Update();
    protected abstract Watt ComputePowerDraw();

    // Message bus
    protected IEventBus<DeviceMessage>? MessageBus { get; private set; }
    public virtual void SetEventBus(IEventBus<DeviceMessage> eventBus) => MessageBus = eventBus;
    protected void PublishMessage(DeviceMessage message) => MessageBus?.Publish(this, message);

    // Power bus
    protected IEventBus<Watt>? PowerBus { get; private set; }
    public virtual void SetEventBus(IEventBus<Watt> eventBus) => PowerBus = eventBus;
    protected void PublishPowerUsage(Watt power) => PowerBus?.Publish(this, power);
}
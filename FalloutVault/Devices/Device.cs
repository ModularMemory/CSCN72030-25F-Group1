using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    // IDevice members
    public abstract DeviceId Id { get; }
    public abstract DeviceType Type { get; }

    public abstract void Update();
    public abstract void SendCommand(DeviceCommand command);

    // Message bus
    protected IEventBus<DeviceMessage>? MessageBus { get; private set; }
    public virtual void SetEventBus(IEventBus<DeviceMessage> eventBus) => MessageBus = eventBus;
    protected void PublishMessage(DeviceMessage message) => MessageBus?.Publish(this, message);

    // Power bus
    protected IEventBus<Watt>? PowerBus { get; private set; }
    public virtual void SetEventBus(IEventBus<Watt> eventBus) => PowerBus = eventBus;
}
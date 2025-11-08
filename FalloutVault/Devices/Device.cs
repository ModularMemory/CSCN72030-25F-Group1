using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices;

public abstract class Device : IDevice
{
    // IDevice members
    public abstract DeviceId Id { get; }

    private WattHours _powerDraw;

    public WattHours PowerDraw
    {
        get => _powerDraw;
        protected set
        {
            if (!SetField(ref _powerDraw, value)) return;

            PublishPowerUsage(_powerDraw);
        }
    }

    public abstract void Update();
    protected abstract WattHours ComputePowerDraw();

    // Message bus
    protected IEventBus<DeviceMessage>? MessageBus { get; private set; }
    public virtual void SetEventBus(IEventBus<DeviceMessage> eventBus) => MessageBus = eventBus;
    protected void PublishMessage(DeviceMessage message) => MessageBus?.Publish(this, message);

    // Power bus
    protected IEventBus<WattHours>? PowerBus { get; private set; }
    public virtual void SetEventBus(IEventBus<WattHours> eventBus) => PowerBus = eventBus;
    protected void PublishPowerUsage(WattHours power) => PowerBus?.Publish(this, power);
}
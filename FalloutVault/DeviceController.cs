using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing;
using JetBrains.Annotations;

namespace FalloutVault;

public sealed class DeviceController
{
    // Fields
    private readonly Dictionary<DeviceId, IDevice> _devices = [];
    private readonly DeviceEventBus _messageBus = new();
    private readonly PowerEventBus _powerEventBus = new();

    // Properties
    public IReadOnlyCollection<IDevice> Devices => _devices.Values;

    // Constructors
    public DeviceController() { }

    // Methods
    public DeviceController AddDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);

        _devices.Add(new DeviceId(device), device);

        return this;
    }

    // Nested classes
    private readonly record struct DeviceId([UsedImplicitly] string Name, [UsedImplicitly] string Zone)
    {
        public DeviceId(IDevice device) : this(device.Name, device.Zone) { }
    }
}
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
        _devices.Add(device.Id, device);

        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);

        return this;
    }
}
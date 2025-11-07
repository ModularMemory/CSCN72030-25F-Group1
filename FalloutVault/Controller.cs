using FalloutVault.Devices.Interfaces;
using FalloutVault.Eventing;

namespace FalloutVault;

public sealed class Controller
{
    // Fields

    private readonly Dictionary<(string name, string zone), IDevice> _devices = [];
    private readonly DeviceEventBus _messageBus = new();
    private readonly PowerEventBus _powerEventBus = new();

    // Properties

    public IReadOnlyCollection<IDevice> Devices => _devices.Values;
    public string Name { get; }

    // Constructors

    public Controller(string name)
    {
        Name = name;
    }

    // Methods

    public Controller AddDevice<TDevice>(TDevice device) where TDevice : IDevice
    {
        device.SetEventBus(_messageBus);
        device.SetEventBus(_powerEventBus);

        _devices.Add((device.Name, device.Zone), device);

        return this;
    }
}
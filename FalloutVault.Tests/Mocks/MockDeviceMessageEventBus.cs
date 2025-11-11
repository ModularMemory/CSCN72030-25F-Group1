using FalloutVault.Eventing;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Tests.Mocks;

public class MockDeviceMessageEventBus : IEventBus<DeviceMessage>
{
    private readonly List<DeviceMessage> _messages = [];
    private readonly DeviceMessageEventBus _innerBus = new();

    public event EventHandler<DeviceMessage>? Handler;

    public IReadOnlyList<DeviceMessage> Messages => _messages;

    public MockDeviceMessageEventBus()
    {
        _innerBus.Handler += (sender, message) => Handler?.Invoke(sender, message);
    }

    public void Publish(object sender, DeviceMessage data)
    {
        _messages.Add(data);
    }
}
using FalloutVault.Eventing;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Tests.Mocks;

internal class MockDeviceMessageEventBus : MockEventBus<DeviceMessage>
{
    public MockDeviceMessageEventBus()
    {
        InnerBus = new DeviceMessageEventBus();
    }
}
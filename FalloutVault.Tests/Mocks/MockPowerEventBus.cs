using FalloutVault.Eventing;
using FalloutVault.Models;

namespace FalloutVault.Tests.Mocks;

internal class MockPowerEventBus : MockEventBus<Watt>
{
    public MockPowerEventBus()
    {
        InnerBus = new PowerEventBus();
    }
}
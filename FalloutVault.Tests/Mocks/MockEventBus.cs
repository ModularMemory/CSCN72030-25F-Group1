using System.Diagnostics;
using FalloutVault.Eventing.Interfaces;

namespace FalloutVault.Tests.Mocks;

internal abstract class MockEventBus<TMessage> : IEventBus<TMessage>
{
    private readonly List<TMessage> _messages = [];

    public event EventHandler<TMessage>? Handler;

    public IReadOnlyList<TMessage> Messages => _messages;

    protected IEventBus<TMessage>? InnerBus
    {
        get;
        init
        {
            // Unsubscribe
            field?.Handler -= InnerBusOnHandler;

            // Set new and subscribe
            field = value;
            field?.Handler += InnerBusOnHandler;
        }
    }

    private void InnerBusOnHandler(object? sender, TMessage message)
    {
        Handler?.Invoke(sender, message);
    }

    public void Publish(object sender, TMessage data)
    {
        Debug.Assert(InnerBus != null);

        _messages.Add(data);
        InnerBus.Publish(sender, data);
    }

    public void ClearMessages()
    {
        _messages.Clear();
    }
}
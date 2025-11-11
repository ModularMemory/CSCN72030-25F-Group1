using System.Diagnostics;
using FalloutVault.Eventing.Interfaces;

namespace FalloutVault.Tests.Mocks;

internal abstract class MockEventBus<TMessage> : IEventBus<TMessage>
{
    private readonly List<TMessage> _messages = [];
    private readonly IEventBus<TMessage>? _innerBus;

    public event EventHandler<TMessage>? Handler;

    public IReadOnlyList<TMessage> Messages => _messages;

    protected IEventBus<TMessage>? InnerBus
    {
        get => _innerBus;
        init
        {
            if (_innerBus != null) _innerBus.Handler -= InnerBusOnHandler;

            _innerBus = value;

            if (_innerBus != null) _innerBus.Handler += InnerBusOnHandler;
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
}
using System.Diagnostics;
using FalloutVault.Eventing.Interfaces;

namespace FalloutVault.Tests.Mocks;

internal abstract class MockEventBus<TMessage> : IEventBus<TMessage>
{
    private readonly List<TMessage> _messages = [];
    private readonly IEventBus<TMessage> _innerBus = null!;

    public event EventHandler<TMessage>? Handler;

    public IReadOnlyList<TMessage> Messages => _messages;

    protected IEventBus<TMessage> InnerBus
    {
        get => _innerBus;
        init
        {
            _innerBus = value;
            _innerBus.Handler += (sender, message) => Handler?.Invoke(sender, message);
        }
    }

    public void Publish(object sender, TMessage data)
    {
        Debug.Assert(InnerBus != null);

        _messages.Add(data);
        InnerBus.Publish(sender, data);
    }
}
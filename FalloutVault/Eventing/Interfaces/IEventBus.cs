namespace FalloutVault.Eventing.Interfaces;

public interface IEventBus<TMessage>
{
    event EventHandler<TMessage>? Handler;

    void Publish(object sender, TMessage data);
}
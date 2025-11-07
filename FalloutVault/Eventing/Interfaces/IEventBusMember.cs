namespace FalloutVault.Eventing.Interfaces;

public interface IEventBusMember<TMessage>
{
    void SetEventBus(IEventBus<TMessage> eventBus);
}
namespace Applications.Usecase;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class;
}
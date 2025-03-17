using SharedKernel;

namespace Applications.Usecase.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<Entity> entitiesWithEvents);
}
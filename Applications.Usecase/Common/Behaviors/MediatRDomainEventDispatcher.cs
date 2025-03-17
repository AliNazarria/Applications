using Applications.Usecase.Common.Interfaces;
using MediatR;
using SharedKernel;

namespace Applications.Usecase.Common.Behaviors;

public class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public MediatRDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAndClearEvents(IEnumerable<Entity> entitiesWithEvents)
    {
        foreach (Entity entitiesWithEvent in entitiesWithEvents)
        {
            IDomainEvent[] array = entitiesWithEvent.DomainEvents.ToArray();
            entitiesWithEvent.ClearDomainEvents();
            IDomainEvent[] array2 = array;
            foreach (IDomainEvent notification in array2)
            {
                await _mediator.Publish(notification).ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }
}
using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationSetEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ) : INotificationHandler<ApplicationUpdateEvent>
{
    public async Task Handle(ApplicationUpdateEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}
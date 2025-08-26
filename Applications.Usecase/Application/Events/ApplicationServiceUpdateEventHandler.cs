using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationServiceUpdateEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ) : INotificationHandler<ApplicationServiceUpdateEvent>

{
    public async Task Handle(ApplicationServiceUpdateEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}
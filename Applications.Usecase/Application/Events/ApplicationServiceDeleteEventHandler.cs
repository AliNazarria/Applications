using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationServiceDeleteEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ) : INotificationHandler<ApplicationServiceDeleteEvent>

{
    public async Task Handle(ApplicationServiceDeleteEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}

using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationDeleteEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ): INotificationHandler<ApplicationDeleteEvent>
{
    public async Task Handle(ApplicationDeleteEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}

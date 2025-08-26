using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationAddEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ) : INotificationHandler<ApplicationAddEvent>
{
    public async Task Handle(ApplicationAddEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}
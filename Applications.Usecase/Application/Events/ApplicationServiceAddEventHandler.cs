using Applications.Domain.Application.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationServiceAddEventHandler(
    IEventBus eventBus,
    ILoggerServiceProvider loggerService
    ) : INotificationHandler<ApplicationServiceAddEvent>

{
    public async Task Handle(ApplicationServiceAddEvent notification, CancellationToken cancellationToken)
    {
        await eventBus.PublishAsync(new { });
        await loggerService.LogUserActivity();
    }
}
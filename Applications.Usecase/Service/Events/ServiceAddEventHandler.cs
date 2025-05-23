using Applications.Domain.Service.Events;

namespace Applications.Usecase.Service.Events;

public class ServiceAddEventHandler(ILoggerServiceProvider loggerService)
    : INotificationHandler<ServiceAddEvent>
{
    public async Task Handle(ServiceAddEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}

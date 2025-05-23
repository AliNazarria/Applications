using Applications.Domain.Service.Events;

namespace Applications.Usecase.Service.Events;

public class ServiceSetEventHandler(ILoggerServiceProvider loggerService)
    : INotificationHandler<ServiceUpdateEvent>
{
    public async Task Handle(ServiceUpdateEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}

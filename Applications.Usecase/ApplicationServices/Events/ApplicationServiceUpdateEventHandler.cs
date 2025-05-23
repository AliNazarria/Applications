using Common.Domain.Entities.Events;

namespace Applications.Usecase.ApplicationServices.Events;

public class ApplicationServiceUpdateEventHandler(
    ILoggerServiceProvider loggerService
    )
    : INotificationHandler<ApplicationServiceUpdateEvent>
{
    public async Task Handle(ApplicationServiceUpdateEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
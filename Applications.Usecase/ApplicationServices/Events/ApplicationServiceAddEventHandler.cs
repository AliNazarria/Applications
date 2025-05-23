using Common.Domain.Entities.Events;

namespace Applications.Usecase.ApplicationServices.Events;

public class ApplicationServiceAddEventHandler(
    ILoggerServiceProvider loggerService
    )
    : INotificationHandler<ApplicationServiceAddEvent>
{
    public async Task Handle(ApplicationServiceAddEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
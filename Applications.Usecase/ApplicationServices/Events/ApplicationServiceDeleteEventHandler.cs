using Common.Domain.Entities.Events;

namespace Applications.Usecase.ApplicationServices.Events;

public class ApplicationServiceDeleteEventHandler(
    ILoggerServiceProvider loggerService
    )
    : INotificationHandler<ApplicationServiceDeleteEvent>
{
    public async Task Handle(ApplicationServiceDeleteEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}

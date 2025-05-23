using Common.Domain.Entities.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationSetEventHandler(
    ILoggerServiceProvider loggerService,
    IUserContextProvider userContext, 
    IDateTimeProvider dateTime)
    : INotificationHandler<ApplicationUpdateEvent>
{
    public async Task Handle(ApplicationUpdateEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
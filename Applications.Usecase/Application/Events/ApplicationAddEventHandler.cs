using Common.Domain.Entities.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationAddEventHandler(
    ILoggerServiceProvider loggerService,
    IUserContextProvider userContext,
    IDateTimeProvider dateTime)
    : INotificationHandler<ApplicationAddEvent>
{
    public async Task Handle(ApplicationAddEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
using Common.Domain.Entities.Events;

namespace Applications.Usecase.Application.Events;

public class ApplicationDeleteEventHandler(
    ILoggerServiceProvider loggerService,
    IUserContextProvider userContext, IDateTimeProvider dateTime)
    : INotificationHandler<ApplicationDeleteEvent>
{
    public async Task Handle(ApplicationDeleteEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}

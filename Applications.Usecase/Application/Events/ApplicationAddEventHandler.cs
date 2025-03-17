using Applications.Domain.Application.Events;
using Applications.Usecase.Common.Interfaces;
using MediatR;

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
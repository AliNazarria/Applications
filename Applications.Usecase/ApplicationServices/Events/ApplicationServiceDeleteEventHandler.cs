using Applications.Domain.Application.Events;
using Applications.Usecase.Common.Interfaces;
using MediatR;

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

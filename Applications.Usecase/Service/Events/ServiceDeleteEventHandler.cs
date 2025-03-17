using Applications.Domain.Service.Events;
using Applications.Usecase.Common.Interfaces;
using MediatR;

namespace Applications.Usecase.Service.Events;

public class ServiceDeleteEventHandler(ILoggerServiceProvider loggerService) :
    INotificationHandler<ServiceDeleteEvent>
{
    public async Task Handle(ServiceDeleteEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
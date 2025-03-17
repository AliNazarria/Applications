using Applications.Domain.Application.Events;
using Applications.Usecase.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Usecase.Application.Events;

public class ApplicationSetEventHandler(
    ILoggerServiceProvider loggerService,
    IUserContextProvider userContext, IDateTimeProvider dateTime)
    : INotificationHandler<ApplicationUpdateEvent>
{
    public async Task Handle(ApplicationUpdateEvent notification, CancellationToken cancellationToken)
    {
        await loggerService.LogUserActivity();
    }
}
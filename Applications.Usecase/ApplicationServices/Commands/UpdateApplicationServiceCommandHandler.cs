using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Commands;

public class UpdateApplicationServiceCommandHandler(
    [FromKeyedServices("proxy")] IGenericRepository<domain.ApplicationService, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider
    )
    : IRequestHandler<UpdateApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(UpdateApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var applicationService = await repository.GetAsync(request.ID, token: cancellationToken);
        if (applicationService is null)
            return domain.Errors.ApplicationServiceNotFound();

        var appResult = applicationService.Update(request.application,
            request.service,
            request.active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        if (appResult.IsError)
            return appResult.Errors;

        var result = await repository.UpdateAsync(applicationService);
        if (result > 0)
            return result;

        return Errors.ApplicationServiceSetFailed();
    }
}

using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Commands;

public class DeleteApplicationServiceCommandHandler(
    IGenericRepository<domain.ApplicationService, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider
    )
    : IRequestHandler<DeleteApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var applicationService = await repository.GetAsync(request.ID, token: cancellationToken);
        if (applicationService is null)
            return Error.NotFound(description: Resources.ResourceKey.ApplicationService.NotFound);

        var appResult = applicationService.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());
        if (appResult.IsError)
            return appResult.Errors;

        var result = await repository.UpdateAsync(applicationService);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.ApplicationService.DeletedFailed);
    }
}
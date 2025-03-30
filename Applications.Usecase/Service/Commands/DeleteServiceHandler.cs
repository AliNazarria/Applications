using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class DeleteServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Service, int> repository,
    IDateTimeProvider dateTimeProvider,
    IUserContextProvider userContext)
    : IRequestHandler<DeleteServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteServiceCommand request
        , CancellationToken cancellationToken)
    {
        var service = await repository.GetAsync(request.ID);
        if (service is null)
            return Error.NotFound(description: Resources.ResourceKey.Service.NotFound);

        service.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(service);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Service.DeletedFailed);
    }
}

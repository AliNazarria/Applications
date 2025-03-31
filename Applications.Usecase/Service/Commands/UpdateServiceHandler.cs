using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class UpdateServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateServiceCommand, ErrorOr<int>>.Handle(
        UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetAsync(request.ID);
        if (service is null)
            return Errors.ServiceNotFound();

        service.Update(request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(service);
        if (result > 0)
            return result;

        return Errors.ServiceSetFailed();
    }
}
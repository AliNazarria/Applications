using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class AddServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<AddServiceCommand, ErrorOr<int>>.Handle(
        AddServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new domain.Service(
            request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        var result = await repository.InsertAsync(service);
        if (result > 0)
            return result;

        return Errors.ServiceSetFailed();
    }
}

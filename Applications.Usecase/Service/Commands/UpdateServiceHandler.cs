using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class UpdateServiceHandler(
    [FromKeyedServices("proxy")] IGenericRepository<domain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateServiceCommand, ErrorOr<int>>.Handle(
        UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetAsync(request.ID);
        if (service is null)
            return Error.NotFound(description: Resources.ResourceKey.Service.NotFound);

        service.Update(request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(service);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Service.SetFailed);
    }
}
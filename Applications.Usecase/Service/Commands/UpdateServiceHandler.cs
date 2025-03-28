using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class UpdateServiceHandler(
    IGenericRepository<domain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateServiceCommand, ErrorOr<int>>.Handle(
        UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return Error.NotFound(description: Resources.ResourceKey.Service.NotFound);

        app.Update(request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(app);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Service.SetFailed);
    }
}
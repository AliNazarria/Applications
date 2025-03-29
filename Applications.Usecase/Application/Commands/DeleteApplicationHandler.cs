using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Commands;

public class DeleteApplicationHandler(
    [FromKeyedServices("proxy")] IGenericRepository<domain.Application, int> repository,
    IDateTimeProvider dateTimeProvider,
    IUserContextProvider userContext)
    : IRequestHandler<DeleteApplicationCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteApplicationCommand request
        , CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return Error.NotFound(description: Resources.ResourceKey.Application.NotFound);

        app.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(app);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Application.DeletedFailed);
    }
}
using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Commands;

public class UpdateApplicationHandler(
    IGenericRepository<domain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateApplicationCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateApplicationCommand, ErrorOr<int>>.Handle(
        UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return Error.NotFound(description: Resources.ResourceKey.Application.NotFound);

        app.Update(request.Key,
            request.Title,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound(),
            request.Comment,
            request.LogoAddress);

        var result = await repository.UpdateAsync(app);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Application.SetFailed);
    }
}
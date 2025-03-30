using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Commands;

public class AddApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddApplicationCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<AddApplicationCommand, ErrorOr<int>>.Handle(
        AddApplicationCommand request, CancellationToken cancellationToken)
    {
        var app = new domain.Application(
            request.Key,
            request.Title,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound(),
            request.Comment,
            request.LogoAddress);
        var result = await repository.InsertAsync(app);
        if (result > 0)
            return result;

        return Error.Failure(description: Resources.ResourceKey.Application.SetFailed);
    }
}
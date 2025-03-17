using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Service;

namespace Applications.Usecase.Service.Commands;

public class DeleteServiceHandler(
    IGenericRepository<domain.Service, int> repository,
    IDateTimeProvider dateTimeProvider,
    IResourceLocalizer localizer,
    IUserContextProvider userContext)
    : IRequestHandler<DeleteServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteServiceCommand request
        , CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return Error.NotFound(description: localizer.Localize(Resources.ResourceKey.Service.NotFound));

        app.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(app);
        if (result > 0)
            return result;

        return Error.Failure(description: localizer.Localize(Resources.ResourceKey.Service.DeletedFailed));
    }
}

using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.Application.Commands;

public class DeleteApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Application, int> repository,
    IDateTimeProvider dateTimeProvider,
    IUserContextProvider userContext)
    : IRequestHandler<DeleteApplicationCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteApplicationCommand request
        , CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return Errors.ApplicationNotFound();

        app.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(app);
        if (result > 0)
            return result;

        return Errors.ApplicationDeletedFailed();
    }
}
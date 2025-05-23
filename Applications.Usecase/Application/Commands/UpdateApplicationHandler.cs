namespace Applications.Usecase.Application.Commands;

public class UpdateApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateApplicationCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateApplicationCommand, ErrorOr<int>>.Handle(
        UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var app = await repository.GetAsync(request.ID);
        if (app is null)
            return ApplicationErrors.ApplicationNotFound();

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

        return ApplicationErrors.ApplicationSetFailed();
    }
}
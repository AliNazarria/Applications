namespace Applications.Usecase.Application.Commands;

public class AddApplicationHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddApplicationCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<AddApplicationCommand, ErrorOr<int>>.Handle(
        AddApplicationCommand request, CancellationToken cancellationToken)
    {
        var app = new appDomain.Application(
            request.Key,
            request.Title,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound(),
            request.Comment,
            request.LogoAddress);
        var result = await repository.InsertAsync(app);
        if (result is null)
            return ApplicationErrors.ApplicationSetFailed();

        return result.ID;
    }
}
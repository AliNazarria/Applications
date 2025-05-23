namespace Applications.Usecase.Service.Commands;

public class AddServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<AddServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<AddServiceCommand, ErrorOr<int>>.Handle(
        AddServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new serviceDomain.Service(
            request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        var result = await repository.InsertAsync(service);
        if (result is null)
            return ServiceErrors.ServiceSetFailed();

        return result.ID;
    }
}
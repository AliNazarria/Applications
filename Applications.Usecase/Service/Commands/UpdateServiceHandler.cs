namespace Applications.Usecase.Service.Commands;

public class UpdateServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateServiceCommand, ErrorOr<int>>
{
    async Task<ErrorOr<int>> IRequestHandler<UpdateServiceCommand, ErrorOr<int>>.Handle(
        UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetAsync(request.ID);
        if (service is null)
            return ServiceErrors.ServiceNotFound();

        service.Update(request.Key,
            request.Name,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(service);
        if (result > 0)
            return result;

        return ServiceErrors.ServiceSetFailed();
    }
}
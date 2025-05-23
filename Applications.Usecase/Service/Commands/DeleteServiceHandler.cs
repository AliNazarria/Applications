namespace Applications.Usecase.Service.Commands;

public class DeleteServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<serviceDomain.Service, int> repository,
    IDateTimeProvider dateTimeProvider,
    IUserContextProvider userContext)
    : IRequestHandler<DeleteServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteServiceCommand request
        , CancellationToken cancellationToken)
    {
        var service = await repository.GetAsync(request.ID);
        if (service is null)
            return ServiceErrors.ServiceNotFound();

        service.Delete(userContext.UserID, dateTimeProvider.NowTimeStampInSecound());

        var result = await repository.UpdateAsync(service);
        if (result > 0)
            return result;

        return ServiceErrors.ServiceDeletedFailed();
    }
}

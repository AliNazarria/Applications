namespace Applications.Usecase.ApplicationServices.Commands;

public class UpdateApplicationServiceCommandHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.ApplicationService, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider
    )
    : IRequestHandler<UpdateApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(UpdateApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var applicationService = await repository.GetAsync(request.ID, token: cancellationToken);
        if (applicationService is null)
            return appDomain.Errors.ApplicationServiceNotFound();

        var appResult = applicationService.Update(request.ApplicationID,
            request.ServiceID,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        if (appResult.IsError)
            return appResult.Errors;

        var result = await repository.UpdateAsync(applicationService);
        if (result > 0)
            return result;

        return ApplicationServiceErrors.ApplicationServiceSetFailed();
    }
}

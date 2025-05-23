
namespace Applications.Usecase.ApplicationServices.Commands;

public class AddApplicationServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<appDomain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider
    )
    : IRequestHandler<AddApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var option = FindOptions<appDomain.Application>.SetOptions();
        option.Includes = [a => a.Services.Where(x => x.Deleted == false)];
        var application = await repository.GetAsync(request.ApplicationID, option, cancellationToken);
        if (application is null)
            return Application.ApplicationErrors.ApplicationNotFound();

        var addServiceResult = application.AddService(request.ServiceID,
            request.Active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        if (addServiceResult.IsError)
            return addServiceResult.Errors;

        var result = await repository.UpdateAsync(application);
        if (result > 0)
            return result;

        return ApplicationServiceErrors.ApplicationServiceSetFailed();
    }
}
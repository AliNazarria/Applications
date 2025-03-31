using Applications.Usecase.Common;
using Applications.Usecase.Common.Interfaces;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Commands;

public class AddApplicationServiceHandler(
    [FromKeyedServices(Constants.Proxy)] IGenericRepository<domain.Application, int> repository,
    IUserContextProvider userContext,
    IDateTimeProvider dateTimeProvider
    )
    : IRequestHandler<AddApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var option = FindOptions<domain.Application>.SetOptions();
        option.Includes = [a => a.Services.Where(x => x.Deleted == false)];
        var application = await repository.GetAsync(request.application, option, cancellationToken);
        if (application is null)
            return Application.Errors.ApplicationNotFound();

        var addServiceResult = application.AddService(request.service,
            request.active,
            userContext.UserID,
            dateTimeProvider.NowTimeStampInSecound());
        if (addServiceResult.IsError)
            return addServiceResult.Errors;

        var result = await repository.UpdateAsync(application);
        if (result > 0)
            return result;

        return Errors.ApplicationServiceSetFailed();
    }
}
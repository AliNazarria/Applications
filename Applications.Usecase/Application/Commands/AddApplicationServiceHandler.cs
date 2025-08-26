
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Commands;

public class AddApplicationServiceHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<AddApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateApplicationSpecification(request.ApplicationID);
        var application = await repository.GetAsync(updateSpec, cancellationToken);
        if (application is null)
            return ApplicationErrors.ApplicationNotFound();

        var addServiceResult = application.AddService(
            request.ApplicationService.ServiceID,
            request.ApplicationService.Active);
        if (addServiceResult.IsError)
            return addServiceResult.Errors;

        var result = await repository.UpdateAsync(application);
        if (result)
            return application.ID;

        return ApplicationErrors.ApplicationServiceSetFailed();
    }
}
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Commands;

public class UpdateApplicationServiceCommandHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<UpdateApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(UpdateApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateApplicationSpecification(request.ApplicationID);
        var app = await repository.GetAsync(updateSpec, cancellationToken);
        if (app is null)
            return ApplicationErrors.ApplicationNotFound();

        var updateResult = app.UpdateService(
            request.ApplicationServiceID,
            request.ApplicationService.ServiceID,
            request.ApplicationService.Active);
        if (updateResult.IsError)
            return updateResult.Errors;

        var result = await repository.UpdateAsync(app);
        if (result)
            return app.ID;

        return ApplicationErrors.ApplicationServiceSetFailed();
    }
}
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Commands;

public class DeleteApplicationServiceCommandHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<DeleteApplicationServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateApplicationSpecification(request.ApplicationID);
        var app = await repository.GetAsync(updateSpec, cancellationToken);
        if (app is null)
            return Application.ApplicationErrors.ApplicationNotFound();

        var delResult = app.DeleteService(request.ApplicationServiceID);
        if (delResult.IsError)
            return delResult.Errors;

        var result = await repository.UpdateAsync(app);
        if (result)
            return app.ID;

        return ApplicationErrors.ApplicationServiceDeleteFailed();
    }
}
using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Commands;

public class DeleteApplicationHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<DeleteApplicationCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteApplicationCommand request
        , CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateApplicationSpecification(request.ID);
        var app = await repository.GetAsync(updateSpec, cancellationToken);
        if (app is null)
            return ApplicationErrors.ApplicationNotFound();

        var deleteResult = app.Delete();
        if (deleteResult.IsError)
            return deleteResult.Errors;

        var result = await repository.UpdateAsync(app);
        if (result)
            return app.ID;

        return ApplicationErrors.ApplicationDeletedFailed();
    }
}
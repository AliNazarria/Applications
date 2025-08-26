using Applications.Usecase.Application.Specifications;

namespace Applications.Usecase.Application.Commands;

public class UpdateApplicationHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<UpdateApplicationCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateApplicationSpecification(request.ID);
        var app = await repository.GetAsync(updateSpec, cancellationToken);
        if (app is null)
            return ApplicationErrors.ApplicationNotFound();

        var updateResult = app.Update(request.Application.Key,
            request.Application.Title,
            request.Application.Active,
            request.Application.Description,
            request.Application.LogoAddress);
        if (updateResult.IsError)
            return updateResult.Errors;

        var result = await repository.UpdateAsync(app);
        if (result)
            return app.ID;

        return ApplicationErrors.ApplicationSetFailed();
    }
}
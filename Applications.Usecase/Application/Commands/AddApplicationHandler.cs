
namespace Applications.Usecase.Application.Commands;

public class AddApplicationHandler(
    IGenericRepository<appDomain.Application, int> repository
    ) : IRequestHandler<AddApplicationCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddApplicationCommand request, CancellationToken cancellationToken)
    {
        var newApplicationResult = appDomain.Application.CreateInstance(
            request.Application.Key,
            request.Application.Title,
            request.Application.Active,
            request.Application.Description,
            request.Application.LogoAddress);
        if (newApplicationResult.IsError)
            return newApplicationResult.Errors;

        var result = await repository.InsertAsync(newApplicationResult.Value);
        if (result is null)
            return ApplicationErrors.ApplicationSetFailed();

        return result.ID;
    }
}
namespace Applications.Usecase.Application.Queries;

public class GetAllAplicationHandler(
    [FromKeyedServices(Common.Usecase.Constants.Cached)] IApplicationRepository repository
    ) : IRequestHandler<GetAllAplicationQuery, ErrorOr<List<ApplicationDTO>>>
{
    public async Task<ErrorOr<List<ApplicationDTO>>> Handle(GetAllAplicationQuery request, CancellationToken cancellationToken)
    {
        return await repository.ApplicationGetlistAsync();
    }
}
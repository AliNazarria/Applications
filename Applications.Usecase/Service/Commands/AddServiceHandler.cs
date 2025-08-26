namespace Applications.Usecase.Service.Commands;

public class AddServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository
    ) : IRequestHandler<AddServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(AddServiceCommand request, CancellationToken cancellationToken)
    {
        var newServiceResult = serviceDomain.Service.CreateInstance(
            request.Service.Key,
            request.Service.Name,
            request.Service.Active);
        if (newServiceResult.IsError)
            return newServiceResult.Errors;

        var result = await repository.InsertAsync(newServiceResult.Value);
        if (result is null)
            return ServiceErrors.ServiceSetFailed();

        return result.ID;
    }
}
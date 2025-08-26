using Applications.Usecase.Service.Specifications;

namespace Applications.Usecase.Service.Commands;

public class DeleteServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository
    ) : IRequestHandler<DeleteServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(DeleteServiceCommand request
        , CancellationToken cancellationToken)
    {
        var getSpec = new GetServiceSpecification(request.ID);
        var service = await repository.GetAsync(getSpec, cancellationToken);
        if (service is null)
            return ServiceErrors.ServiceNotFound();

        var delResult = service.Delete();
        if (delResult.IsError)
            return delResult.Errors;

        var result = await repository.UpdateAsync(service);
        if (result)
            return service.ID;

        return ServiceErrors.ServiceDeletedFailed();
    }
}
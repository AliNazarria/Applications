using Applications.Usecase.Service.Specifications;

namespace Applications.Usecase.Service.Commands;

public class UpdateServiceHandler(
    IGenericRepository<serviceDomain.Service, int> repository
    ) : IRequestHandler<UpdateServiceCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var updateSpec = new UpdateServiceSpecification(request.ID);
        var service = await repository.GetAsync(updateSpec);
        if (service is null)
            return ServiceErrors.ServiceNotFound();

        var updateResult = service.Update(request.Service.Key,
            request.Service.Name,
            request.Service.Active);
        if (updateResult.IsError)
            return updateResult.Errors;

        var result = await repository.UpdateAsync(service);
        if (result)
            return service.ID;

        return ServiceErrors.ServiceSetFailed();
    }
}
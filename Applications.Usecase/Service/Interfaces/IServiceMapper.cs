using Applications.Usecase.Service.Dto;

namespace Applications.Usecase.Service.Interfaces;

public interface IServiceMapper
{
    ServiceDTO ToDto(Domain.Service.Service service);
    List<ServiceDTO> ToDto(List<Domain.Service.Service> service);
    PaginatedListDTO<ServiceDTO> ToDto(PaginatedListDTO<Domain.Service.Service> service);
}

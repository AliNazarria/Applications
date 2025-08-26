using Applications.Usecase.Application.Dto;

namespace Applications.Usecase.Application.Interfaces;

public interface IApplicationMapper
{
    ApplicationDTO ToDto(appDomain.Application app);
    List<ApplicationDTO> ToDto(List<appDomain.Application> app);
    PaginatedListDTO<ApplicationDTO> ToDto(PaginatedListDTO<appDomain.Application> app);

    ApplicationServiceDTO ToDto(appDomain.ApplicationService service);
    List<ApplicationServiceDTO> ToDto(List<appDomain.ApplicationService> service);
    PaginatedListDTO<ApplicationServiceDTO> ToDto(PaginatedListDTO<appDomain.ApplicationService> app);
}
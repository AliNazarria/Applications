using Applications.Usecase.Service.Dto;
using Applications.Usecase.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Usecase.Service;

public class ServiceMapper : IServiceMapper
{
    public ServiceDTO ToDto(Domain.Service.Service service) =>
        new(service.Key.Value, service.Name.Value)
        {
            ID = service.ID,
            Active = service.Active,
            Deleted = service.Deleted,
            Created_At = service.Created_At,
            Created_By = service.Created_By,
            Updated_At = service.Updated_At,
            Updated_By = service.Updated_By,
        };
    public List<ServiceDTO> ToDto(List<Domain.Service.Service> service) =>
        service.ConvertAll(ToDto);
    public PaginatedListDTO<ServiceDTO> ToDto(PaginatedListDTO<Domain.Service.Service> service) =>
        new PaginatedListDTO<ServiceDTO>(service.Items.ConvertAll(ToDto), service.TotalCount, service.PageNumber, service.Items.Count);
}

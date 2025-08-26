using Applications.Usecase.Application.Dto;
using Applications.Usecase.Application.Interfaces;

namespace Applications.Usecase.Application;

public class ApplicationMapper : IApplicationMapper
{
    public ApplicationServiceDTO ToDto(appDomain.ApplicationService service) =>
        new(service.ApplicationID,
            service.Application?.Title.Value, 
            service.ServiceID,
            service.Service?.Name.Value)
        {
            ID = service.ID,
            Active = service.Active,
            Deleted = service.Deleted,
            Created_At = service.Created_At,
            Created_By = service.Created_By,
            Updated_At = service.Updated_At,
            Updated_By = service.Updated_By,
        };
    public List<ApplicationServiceDTO> ToDto(List<appDomain.ApplicationService> service) =>
        service.ConvertAll(ToDto);
    public PaginatedListDTO<ApplicationServiceDTO> ToDto(PaginatedListDTO<appDomain.ApplicationService> app) =>
        new PaginatedListDTO<ApplicationServiceDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
    public ApplicationDTO ToDto(appDomain.Application app) =>
        new(app.Key.Value,
            app.Title.Value,
            app.Description.Value,
            app.LogoAddress.Value,
            app.Services.ConvertAll(ToDto))
        {
            ID = app.ID,
            Active = app.Active,
            Deleted = app.Deleted,
            Created_At = app.Created_At,
            Created_By = app.Created_By,
            Updated_At = app.Updated_At,
            Updated_By = app.Updated_By,
        };
    public List<ApplicationDTO> ToDto(List<appDomain.Application> app) =>
            app.ConvertAll(ToDto);
    public PaginatedListDTO<ApplicationDTO> ToDto(PaginatedListDTO<appDomain.Application> app) =>
        new PaginatedListDTO<ApplicationDTO>(app.Items.ConvertAll(ToDto), app.TotalCount, app.PageNumber, app.Items.Count);
}

namespace Applications.Usecase.Application.Specifications;

public class BaseApplicationSpecification : Specification<appDomain.Application>
{
    public BaseApplicationSpecification(int page, int size)
        : this()
    {
        ApplyPaging(page, size);
    }
    public BaseApplicationSpecification(bool asNoTrackingEnabled = true)
    {
        if (asNoTrackingEnabled)
            QueryMode();

        AddInclude(e => e.Services);
        //AddInclude($"{nameof(appDomain.Application.Services)}.{nameof(appDomain.ApplicationService.Application)}");
        AddInclude($"{nameof(appDomain.Application.Services)}.{nameof(appDomain.ApplicationService.Service)}");
    }
}
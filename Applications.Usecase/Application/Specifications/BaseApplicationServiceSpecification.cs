namespace Applications.Usecase.Application.Specifications;

public class BaseApplicationServiceSpecification
    : Specification<appDomain.ApplicationService>
{
    public BaseApplicationServiceSpecification(int page, int size)
        : this()
    {
        ApplyPaging(page, size);
    }
    public BaseApplicationServiceSpecification(bool asNoTrackingEnabled = true)
    {
        if (asNoTrackingEnabled)
            QueryMode();

        AddInclude(e => e.Application);
        AddInclude(e => e.Service);
    }
}
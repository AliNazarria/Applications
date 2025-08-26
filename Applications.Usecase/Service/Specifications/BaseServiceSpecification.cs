namespace Applications.Usecase.Service.Specifications;

public class BaseServiceSpecification : Specification<serviceDomain.Service>
{
    public BaseServiceSpecification(int page, int size)
       : this()
    {
        ApplyPaging(page, size);
    }
    public BaseServiceSpecification(bool asNoTrackingEnabled = true)
    {
        if (asNoTrackingEnabled)
            QueryMode();
    }
}
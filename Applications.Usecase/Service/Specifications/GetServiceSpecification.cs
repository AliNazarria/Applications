namespace Applications.Usecase.Service.Specifications;

public class GetServiceSpecification: BaseServiceSpecification
{
    public GetServiceSpecification(int id)
    {
        SetCriteria(x => x.ID == id);
    }
}
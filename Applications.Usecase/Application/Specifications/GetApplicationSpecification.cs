namespace Applications.Usecase.Application.Specifications;

public class GetApplicationSpecification : BaseApplicationSpecification
{
    public GetApplicationSpecification(int id)
        : base()
    {
        SetCriteria(c => c.ID == id);
    }
}
namespace Applications.Usecase.Application.Specifications;

public class UpdateApplicationSpecification : BaseApplicationSpecification
{
    public UpdateApplicationSpecification(int id)
    : base(asNoTrackingEnabled: false)
    {
        SetCriteria(c => c.ID == id);
    }
}
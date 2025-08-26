namespace Applications.Usecase.Service.Specifications;

public class UpdateServiceSpecification : BaseServiceSpecification
{
    public UpdateServiceSpecification(int id)
        : base(asNoTrackingEnabled: false)
    {
        SetCriteria(x => x.ID == id);
    }
}
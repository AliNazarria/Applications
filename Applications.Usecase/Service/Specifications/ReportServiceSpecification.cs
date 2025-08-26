namespace Applications.Usecase.Service.Specifications;

public class ReportServiceSpecification : BaseServiceSpecification
{
    public ReportServiceSpecification() : base()
    {
    }
    public ReportServiceSpecification(int page, int size, ReportFilterDTO reportFilter)
        : base(page, size)
    {
        var predicateResult = PredicateBuilder.MakeNestedPredicate<serviceDomain.Service>(reportFilter?.Filter);
        SetCriteria(predicateResult.Value);
        this.ApplyDynamicSorting(reportFilter?.OrderBy);
    }
}
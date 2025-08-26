namespace Applications.Usecase.Application.Specifications;

public class ReportApplicationServiceSpecification
    : BaseApplicationServiceSpecification
{
    public ReportApplicationServiceSpecification() : base()
    {
    }
    public ReportApplicationServiceSpecification(int page, int size, ReportFilterDTO reportFilter)
        : base(page, size)
    {
        var predicateResult = PredicateBuilder.MakeNestedPredicate<appDomain.ApplicationService>(reportFilter?.Filter);
        SetCriteria(predicateResult.Value);
        this.ApplyDynamicSorting(reportFilter?.OrderBy);
    }
}
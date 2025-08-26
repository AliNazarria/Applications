namespace Applications.Usecase.Application.Specifications;

public class ReportApplicationSpecification : BaseApplicationSpecification
{
    public ReportApplicationSpecification(int page, int size, ReportFilterDTO reportFilter)
        : base(page, size)
    {
        var predicateResult = PredicateBuilder.MakeNestedPredicate<appDomain.Application>(reportFilter?.Filter);
        SetCriteria(predicateResult.Value);
        this.ApplyDynamicSorting(reportFilter?.OrderBy);
    }
}
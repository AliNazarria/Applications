using SharedKernel;

namespace Applications.Domain.Common.ValueObjects;

public class CommentValueObject : ValueObject
{
    public string? Value { get; private set; }
    private CommentValueObject() { }
    public CommentValueObject(string? comment)
    {
        this.Value = comment;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString()
    {
        return Value;
    }
}
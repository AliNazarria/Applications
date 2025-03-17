using SharedKernel;

namespace Applications.Domain.Common.ValueObjects;

public class UrlValueObject : ValueObject
{
    public string? Value { get; private set; }
    private UrlValueObject() { }
    public UrlValueObject(string? url)
    {
        this.Value = url;
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
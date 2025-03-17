using SharedKernel;

namespace Applications.Domain.Common.ValueObjects;

public class NameValueObject : ValueObject
{
    public string Value { get; private set; }
    private NameValueObject() { }
    public NameValueObject(string name)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

        if (name.Length > 150)
            throw new ArgumentOutOfRangeException(nameof(name));

        Value = name;
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

using Common.Domain;

namespace Applications.Domain.Service.ValueObjects;

public class NameValueObject : ValueObject
{
    public string Value { get; private set; }
    private NameValueObject() { }
    public NameValueObject(string name)
    {
        if (name is null)
            throw new DomainException(Resources.NameInvalid, "Name is null", 
                new ArgumentNullException(nameof(name)));
        if (name.Length > 150)
            throw new DomainException(Resources.NameInvalid, "Name max length is 150", 
                new ArgumentOutOfRangeException(nameof(name)));

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
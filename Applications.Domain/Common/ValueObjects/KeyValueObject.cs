using SharedKernel;

namespace Applications.Domain.Common.ValueObjects;

public class KeyValueObject : ValueObject
{
    public string Value { get; private set; }
    private KeyValueObject() { }
    public KeyValueObject(string key)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key);
        if (key.Length > 150)
            throw new ArgumentOutOfRangeException(nameof(key));

        this.Value = key;
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
using SharedKernel;

namespace Applications.Domain.Common.ValueObjects;

public class TitleValueObject : ValueObject
{
    public string Value { get; private set; }
    private TitleValueObject() { }
    public TitleValueObject(string title)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(title);
        if (title.Length > 150)
            throw new ArgumentOutOfRangeException(nameof(title));

        this.Value = title;
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
namespace Applications.Usecase.Common.Interfaces;

public interface IUserContextProvider
{
    [HeaderParameter(allowEmptyValue: true, required: false, description: "")]
    int AppID { get; }

    [HeaderParameter(allowEmptyValue: true, required: false, description: "")]
    int ServiceID { get; }

    [HeaderParameter(allowEmptyValue: false, required: true, description: "", example: "")]
    Guid UserID { get; }

    [HeaderParameter(allowEmptyValue: true, required: false, description: "default fa", example: "fa")]
    string Language { get; }

    [HeaderParameter(allowEmptyValue: true, required: false, description: "")]
    string CorrelationId { get; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class HeaderParameterAttribute : Attribute
{
    public bool AllowEmptyValue { get; init; }
    public bool Required { get; init; }
    public string Description { get; init; }
    public string Example { get; init; }
    public HeaderParameterAttribute(bool allowEmptyValue, bool required, string description = "", string example = "")
    {
        this.Required = required;
        this.AllowEmptyValue = allowEmptyValue;
        this.Description = description;
        Example = example;
    }
}
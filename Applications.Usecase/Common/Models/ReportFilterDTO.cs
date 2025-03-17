using System.Text.Json.Serialization;

namespace Applications.Usecase.Common.Models;

public record ReportFilterDTO(
    int Page,
    int Size,
    List<FilterDTO> Filter,
    string OrderBy)
{
}

public record FilterDTO(
    string Key
    , string Value
    , OperationType Operation)
{
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OperationType
{
    Equal = 0,
    NotEqual = 1,
    Greater = 2,
    GreaterEqual = 3,
    Less = 4,
    LessEqual = 5,
    Like = 6,
    StartsWith = 7,
    EndsWith = 8
}
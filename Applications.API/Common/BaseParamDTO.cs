namespace Applications.API.Common;

public record BaseParamDTO()
{
    public bool Active { get; init; }
    public bool Deleted { get; init; }
    public int? Created_By { get; init; }
    public int? Created_At { get; init; }
    public int? Updated_By { get; init; }
    public int? Updated_At { get; init; }
};

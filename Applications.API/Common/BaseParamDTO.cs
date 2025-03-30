namespace Applications.API.Common;

public record BaseInputDTO()
{
    public bool Active { get; init; }
}
public record BaseParamDTO()
{
    public bool Active { get; init; }
    public bool Deleted { get; init; }
    public Guid? Created_By { get; init; }
    public int? Created_At { get; init; }
    public Guid? Updated_By { get; init; }
    public int? Updated_At { get; init; }
};

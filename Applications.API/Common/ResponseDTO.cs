namespace Applications.API.Common;

public record ResponseDTO()
{
    public int Status { get; init; } = 1;
    public string Message { get; init; }
    public string[] Errors { get; init; }
};
public record ResponseDTO<T>(T data = default) : ResponseDTO;
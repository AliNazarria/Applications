namespace Applications.API.Common;

public record ResponseDTO()
{
    public int Status { get; set; } = StatusCodes.Status200OK;
    public string Message { get; init; }
    public string[] Errors { get; init; }
};
public record ResponseDTO<T>(T data = default) : ResponseDTO;
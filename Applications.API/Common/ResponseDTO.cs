using System;

namespace Applications.API.Common;

public record ResponseDTO
{
    protected ResponseDTO() { }
    public int Status { get; init; } = StatusCodes.Status200OK;
    public string Message { get; init; }
    public string[] Errors { get; init; }

    public static ResponseDTO Error(int status,string message, string[] errors)
    {
        return new ResponseDTO
        {
            Status = status,
            Message = message,
            Errors = errors,
        };
    }
    public static ResponseDTO Unauthorized(string message)
    {
        return new ResponseDTO
        {
            Status = StatusCodes.Status401Unauthorized,
            Message = "Unauthorized",
        };
    }
    public static ResponseDTO Exception(Exception ex)
    {
        return new ResponseDTO
        {
            Status = StatusCodes.Status500InternalServerError,
            Message = "Server error",
            Errors = [ex.Message]
        };
    }
};
public record ResponseDTO<T>(T data = default) : ResponseDTO;
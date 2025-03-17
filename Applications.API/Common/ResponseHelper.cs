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
public record ResponseDTO()
{
    public int status { get; init; } = 1;
    public string message { get; init; }
    public string[] errors { get; init; }
};
public record ResponseDTO<T>(T data = default) : ResponseDTO;

public static class ResponseHelper
{
    public static ResponseDTO ToError(int status, string message)
    {
        return new ResponseDTO
        {
            status = status,
            message = message,
            errors = [message],
        };
    }
    public static ResponseDTO<TDto> ToResult<TEntity, TDto>(ErrorOr.ErrorOr<TEntity> result, Func<TDto> mapper)
    {
        if (result.IsError)
            return new ResponseDTO<TDto>()
            {
                status = result.FirstError.NumericType,
                message = result.FirstError.Description,
                errors = result.Errors.Select(x => x.Description).ToArray()
            };

        return new ResponseDTO<TDto>(data: mapper.Invoke());
    }
    public static ResponseDTO<TDto> ToDto<TEntity, TDto>(
        ErrorOr.ErrorOr<TEntity> result,
        Func<TEntity, TDto> mapper)
    {
        if (result.IsError)
            return new ResponseDTO<TDto>()
            {
                status = result.FirstError.NumericType,
                message = result.FirstError.Description,
                errors = result.Errors.Select(x => x.Description).ToArray()
            };

        return new ResponseDTO<TDto>(data: mapper(result.Value));
    }
}
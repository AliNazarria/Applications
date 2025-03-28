using ErrorOr;
using Microsoft.Extensions.Localization;

namespace Applications.API.Common;

public interface IResponseHelper
{
    IResult ErrorResult(Error error);
    IResult CreatedResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TDto> mapper, string location);
    IResult AcceptedResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TDto> mapper, string location);
    IResult OkResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TEntity, TDto> mapper);
    IResult ErrorResult<TEntity>(ErrorOr<TEntity> result);
}
public class ResponseHelper(
    IStringLocalizer<ResponseHelper> localizer
    )
    : IResponseHelper
{
    public IResult ErrorResult(Error error)
    {
        return ErrorResult<int>(error.ToErrorOr<int>());
    }
    public IResult ErrorResult<TEntity>(ErrorOr<TEntity> result)
    {
        var response = new ResponseDTO()
        {
            Status = StatusCodes.Status400BadRequest,
            Message = localizer[result.FirstError.Description],
            Errors = result.Errors.Select(x => localizer[x.Description].ToString()).ToArray()
        };

        switch (result.FirstError.Type)
        {
            case ErrorType.NotFound:
                response.Status = StatusCodes.Status404NotFound;
                return Results.NotFound(response);
            case ErrorType.Unauthorized:
                response.Status = StatusCodes.Status401Unauthorized;
                return Results.Unauthorized();
            case ErrorType.Conflict:
                response.Status = StatusCodes.Status409Conflict;
                return Results.Conflict(response);
            case ErrorType.Validation:
                response.Status = StatusCodes.Status422UnprocessableEntity;
                return Results.UnprocessableEntity(response);
            case ErrorType.Forbidden:
                return Results.Forbid();
            case ErrorType.Unexpected:
            case ErrorType.Failure:
            default:
                response.Status = StatusCodes.Status400BadRequest;
                return Results.BadRequest(response);
        }
    }
    public IResult OkResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TEntity, TDto> mapper)
    {
        if (result.IsError)
            return ErrorResult(result);

        return Results.Ok(new ResponseDTO<TDto>(data: mapper(result.Value)));
    }
    public IResult CreatedResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TDto> mapper, string location)
    {
        if (result.IsError)
            return ErrorResult(result);

        return Results.Created(location, new ResponseDTO<TDto>(data: mapper.Invoke()));
    }
    public IResult AcceptedResult<TEntity, TDto>(ErrorOr<TEntity> result, Func<TDto> mapper, string location)
    {
        if (result.IsError)
            return ErrorResult(result);

        return Results.Accepted(location, new ResponseDTO<TDto>(data: mapper.Invoke()));
    }
}
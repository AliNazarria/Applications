using Applications.Usecase.Common.Interfaces;
using ErrorOr;

namespace Applications.Infrastructure.Common;

public class AuthorizationServiceProvider(IUserContextProvider userContext) 
    : IAuthorizationServiceProvider
{
    public ErrorOr<Success> AuthorizeCurrentUser<T>(
        IAuthorizeableRequest<T> request,
        List<string> requiredRoles,
        List<string> requiredPermissions,
        List<string> requiredPolicies)
    {
        //todo
        if (userContext.UserID == Guid.Empty)
            return Error.Unauthorized();

        return Result.Success;
    }
}
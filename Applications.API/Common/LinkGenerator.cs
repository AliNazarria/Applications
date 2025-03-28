using Applications.API.Util;

namespace Applications.API.Common;

public interface IEndpointLinkGenerator
{
    string Url(HttpContext context, RouteNames routeName, object values);
}
public class EndpointLinkGenerator(
        LinkGenerator linkGenerator
    )
    : IEndpointLinkGenerator
{
    public string Url(HttpContext context, RouteNames routeName, object values)
    {
        return linkGenerator.GetUriByName(context, routeName.ToString(), values) ?? "";
    }
}
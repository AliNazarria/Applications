namespace Applications.API.Util;

public class ApiResources
{
    public const string ApiBasePath = $"api/application";
    public const string PublicBasePath = "public/application";
    public const string Applications = "applications";
    public const string Services = "services";
    public const string ApplicationServices = "applicationservices";
}
public enum RouteNames
{
    None = 0,

    ApplicationGet = 1,
    ApplicationDeletedGet = 2,

    ServiceGet = 3,
    ServiceDeletedGet = 4,

    ApplicationServiceGet = 5,
    ApplicationServiceDeletedGet = 6,
}
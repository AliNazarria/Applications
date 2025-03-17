namespace Applications.Usecase.Application;

public static partial class Permissions
{
    public static class Application
    {
        public const string Update = "set:application";
        public const string Get = "get:application";
        public const string Report = "report:application";
        public const string Delete = "delete:application";
    }
}
namespace Applications.Usecase.Resources;

public class ResourceKey
{
    public const string IdInvalid = "IdInvalid";
    public const string KeyInvalid = "KeyInvalid";
    public const string KeyIsDuplicated = "KeyIsDuplicated";

    public class Application
    {
        public const string TitleInvalid = "TitleInvalid";
        public const string NotFound = "ApplicationNotFound";
        public const string SetFailed = "ApplicationSetFailed";
        public const string DeletedFailed = "ApplicationDeletedFailed";
    }
    public class ApplicationService
    {
        public const string NotFound = "ApplicationServiceNotFound";
        public const string SetFailed = "ApplicationServiceSetFailed";
        public const string DeletedFailed = "ApplicationServiceDeletedFailed";
    }
    public class Service
    {
        public const string NameInvalid = "NameInvalid";
        public const string NotFound = "ServiceNotFound";
        public const string SetFailed = "ServiceSetFailed";
        public const string DeletedFailed = "ServiceDeletedFailed";
    }
}
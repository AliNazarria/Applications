namespace Applications.Usecase.Resources;

public class ResourceKey
{
    public const string IdInvalid = "IdInvalid";
    public const string KeyInvalid = "KeyInvalid";
    public const string KeyIsDuplicated = "KeyIsDuplicated";
    public const string PageSizeInvalid = "PageSizeInvalid";

    public class Application
    {
        public const string TitleInvalid = "TitleInvalid";
        public const string NotFound = "ApplicationNotFound";
        public const string SetFailed = "ApplicationSetFailed";
        public const string DeletedFailed = "ApplicationDeletedFailed";
    }
    public class Service
    {
        public const string NameInvalid = "NameInvalid";
        public const string NotFound = "ServiceNotFound";
        public const string SetFailed = "ServiceSetFailed";
        public const string DeletedFailed = "ServiceDeletedFailed";
    }
}
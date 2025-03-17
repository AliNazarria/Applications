using Applications.Domain.Application.Events;
using Applications.Domain.Common.ValueObjects;
using ErrorOr;
using SharedKernel;
using System.Diagnostics;

namespace Applications.Domain.Application;

[DebuggerDisplay("App {ID}:{Key}:{Title}")]
public class Application : Entity<int>
{
    public KeyValueObject Key { get; private set; }
    public TitleValueObject Title { get; private set; }
    public CommentValueObject Comment { get; private set; }
    public UrlValueObject LogoAddress { get; private set; }
    public virtual ICollection<ApplicationService> Services { get; set; } = [];

    private Application()
    {
    }
    public Application(
        string key,
        string title,
        bool active,
        int userId,
        int createDate,
        string? comment = null,
        string? logo = null)
    {
        this.Key = new KeyValueObject(key);
        this.Title = new TitleValueObject(title);
        this.Comment = new CommentValueObject(comment);
        this.LogoAddress = new UrlValueObject(logo);
        if (active)
            this.Activated();
        this.Create(userId, createDate);
        RegisterDomainEvent(new ApplicationAddEvent(this));
    }

    public ErrorOr<Success> Update(string key,
        string title,
        bool active,
        int userId,
        int updateDate,
        string? comment = null,
        string? logo = null)
    {
        this.Key = new KeyValueObject(key);
        this.Title = new TitleValueObject(title);
        this.Comment = new CommentValueObject(comment);
        this.LogoAddress = new UrlValueObject(logo);
        if (!active)
            this.Deactive();
        this.Update(userId, updateDate);
        RegisterDomainEvent(new ApplicationUpdateEvent(this));
        return Result.Success;
    }

    public ErrorOr<Success> Delete(int user, int deleteDate)
    {
        this.SoftDelete();
        this.Update(user, deleteDate);
        RegisterDomainEvent(new ApplicationDeleteEvent(this));
        return Result.Success;
    }

    public ErrorOr<Success> AddService(int serviceId,
        bool active,
        int user,
        int createDate)
    {
        if (Services.Any(x => x.ServiceID == serviceId && x.Deleted == false))
            return Error.Validation();//todo localizer

        Services.Add(new ApplicationService(this.ID, serviceId, active, user, createDate));
        return Result.Success;
    }
    public ErrorOr<Success> UpdateService(int serviceId,
        bool active,
        int user,
        int updateDate)
    {
        var service = this.Services.FirstOrDefault(x => x.ID == serviceId);
        if (service is null)
            return Error.Validation();//todo localizer

        return service.Update(active, user, updateDate);
    }
    public ErrorOr<Success> DeleteService(int serviceId,
        int user,
        int deleteDate)
    {
        var service = this.Services.FirstOrDefault(x => x.ID == serviceId);
        if (service is null)
            return Error.Validation();//todo localizer

        return service.Delete(user, deleteDate);
    }
}
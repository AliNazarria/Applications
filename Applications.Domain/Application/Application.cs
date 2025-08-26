using Applications.Domain.Application.Events;
using Common.Domain;
using Common.Domain.ValueObjects;
using ErrorOr;
using System.Diagnostics;

namespace Applications.Domain.Application;

[DebuggerDisplay("App {ID}:{Key}:{Title}")]
public class Application : Entity<int>
{
    public KeyValueObject Key { get; private set; }
    public TitleValueObject Title { get; private set; }
    public DescriptionValueObject Description { get; private set; }
    public UrlValueObject LogoAddress { get; private set; }
    public virtual List<ApplicationService> Services { get; set; } = [];

    public static ErrorOr<Application> CreateInstance(string key,
        string title,
        bool active,
        string? desc = null,
        string? logo = null)
    {
        var newApp = new Application(key, title, active, desc, logo);
        return newApp;
    }

    private Application()
    {
    }
    private Application(
        string key,
        string title,
        bool active,
        string? desc = null,
        string? logo = null
        )
    {
        this.Key = new KeyValueObject(key);
        this.Title = new TitleValueObject(title);
        this.Description = new DescriptionValueObject(desc);
        this.LogoAddress = new UrlValueObject(logo);
        if (active)
            this.Activated();
        RegisterDomainEvent(new ApplicationAddEvent(this));
    }
    public ErrorOr<Success> Update(string key,
        string title,
        bool active,
        string? desc = null,
        string? logo = null)
    {
        this.Key = new KeyValueObject(key);
        this.Title = new TitleValueObject(title);
        this.Description = new DescriptionValueObject(desc);
        this.LogoAddress = new UrlValueObject(logo);
        if (!active)
            this.Deactive();
        this.UpdateProperties();
        RegisterDomainEvent(new ApplicationUpdateEvent(this));
        return Result.Success;
    }

    public ErrorOr<Success> Delete()
    {
        this.SoftDelete();
        this.UpdateProperties();
        RegisterDomainEvent(new ApplicationDeleteEvent(this));
        return Result.Success;
    }

    public ErrorOr<Success> AddService(int serviceId, bool active)
    {
        if (Services.Any(x => x.ServiceID == serviceId))
            return Errors.ApplicationServiceIsDuplicate();

        var newServiceResult = ApplicationService.CreateInstance(serviceId, active);
        if (newServiceResult.IsError)
            return newServiceResult.Errors;

        Services.Add(newServiceResult.Value);
        return Result.Success;
    }
    public ErrorOr<Success> UpdateService(int id, int serviceId, bool active)
    {
        var service = this.Services.FirstOrDefault(x => x.ID == id);
        if (service is null)
            return Errors.ApplicationServiceNotFound();

        var result = service.Update(serviceId, active);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }
    public ErrorOr<Success> DeleteService(int id)
    {
        var service = this.Services.FirstOrDefault(x => x.ID == id);
        if (service is null)
            return Errors.ApplicationServiceNotFound();

        var result = service.Delete();
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }
}
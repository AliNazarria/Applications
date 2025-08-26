using Applications.Domain.Application.Events;
using Common.Domain;
using ErrorOr;
using System.Diagnostics;

namespace Applications.Domain.Application;

[DebuggerDisplay("AppService {ApplicationID}:{ServiceID}")]
public class ApplicationService : Entity<int>
{
    public int ApplicationID { get; private set; }
    public virtual Application Application { get; set; }
    public int ServiceID { get; private set; }
    public virtual Service.Service Service { get; set; }

    public static ErrorOr<ApplicationService> CreateInstance(int service, bool active)
    {
        return new ApplicationService(service, active);
    }

    private ApplicationService() { }
    private ApplicationService(int service, bool active)
    {
        this.ServiceID = service;
        if (active)
            this.Activated();
        RegisterDomainEvent(new ApplicationServiceAddEvent(this));
    }
    public ErrorOr<Success> Update(int service, bool active)
    {
        this.ServiceID = service;
        if (!active)
            this.Deactive();

        this.UpdateProperties();
        RegisterDomainEvent(new ApplicationServiceUpdateEvent(this));
        return Result.Success;
    }
    public ErrorOr<Success> Delete()
    {
        this.SoftDelete();
        this.UpdateProperties();
        RegisterDomainEvent(new ApplicationServiceDeleteEvent(this));
        return Result.Success;
    }
}
using Applications.Domain.Application.Events;
using ErrorOr;
using SharedKernel;
using System.Diagnostics;

namespace Applications.Domain.Application;

[DebuggerDisplay("AppService {ApplicationID}:{ServiceID}")]
public class ApplicationService : Entity<int>
{
    public int ApplicationID { get; private set; }
    public virtual Application Application { get; set; }
    public int ServiceID { get; private set; }

    private ApplicationService() { }
    public ApplicationService(int app,
        int service,
        bool active,
        int user,
        int createDate)
    {
        this.ApplicationID = app;
        this.ServiceID = service;
        if (active)
            this.Activated();
        this.Create(user, createDate);
        RegisterDomainEvent(new ApplicationServiceAddEvent(this));
    }
    public ErrorOr<Success> Update(bool active, int user, int updateDate)
    {
        if (!active)
            this.Deactive();

        this.Update(user, updateDate);
        RegisterDomainEvent(new ApplicationServiceUpdateEvent(this));
        return Result.Success;
    }
    public ErrorOr<Success> Delete(int user, int deleteDate)
    {
        this.SoftDelete();
        this.Update(user, deleteDate);
        RegisterDomainEvent(new ApplicationServiceDeleteEvent(this));
        return Result.Success;
    }
}
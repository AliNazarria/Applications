using Applications.Domain.Service.Events;
using Common.Domain;
using System.Diagnostics;
using ErrorOr;
using Applications.Domain.Service.ValueObjects;
using Common.Domain.ValueObjects;

namespace Applications.Domain.Service;

[DebuggerDisplay("Service {ID}:{Key}:{Name}")]
public class Service : Entity<int>
{
    public KeyValueObject Key { get; private set; }
    public NameValueObject Name { get; private set; }

    private Service() { }
    public Service(string key,
        string name,
        bool active,
        Guid userId,
        int createDate)
    {
        this.Key = new KeyValueObject(key);
        this.Name = new NameValueObject(name);
        if (active)
            this.Activated();
        this.Create(createDate, userId);
        RegisterDomainEvent(new ServiceAddEvent(this));
    }

    public ErrorOr<Success> Update(string key,
        string name,
        bool active,
        Guid userId,
        int updateDate)
    {
        this.Key = new KeyValueObject(key);
        this.Name = new NameValueObject(name);
        if (!active)
            this.Deactive();
        this.Update(updateDate, userId);
        RegisterDomainEvent(new ServiceUpdateEvent(this));
        return Result.Success;
    }
    public ErrorOr<Success> Delete(Guid user, int deleteDate)
    {
        this.SoftDelete();
        this.Update(deleteDate, user);
        RegisterDomainEvent(new ServiceDeleteEvent(this));
        return Result.Success;
    }
}
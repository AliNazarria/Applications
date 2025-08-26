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

    public static ErrorOr<Service> CreateInstance(string key,
        string name,
        bool active)
    {
        var newService = new Service(key, name, active);
        return newService;
    }

    private Service() { }
    private Service(string key,
        string name,
        bool active)
    {
        this.Key = new KeyValueObject(key);
        this.Name = new NameValueObject(name);
        if (active)
            this.Activated();
        RegisterDomainEvent(new ServiceAddEvent(this));
    }

    public ErrorOr<Success> Update(string key,
        string name,
        bool active)
    {
        this.Key = new KeyValueObject(key);
        this.Name = new NameValueObject(name);
        if (!active)
            this.Deactive();
        this.UpdateProperties();
        RegisterDomainEvent(new ServiceUpdateEvent(this));
        return Result.Success;
    }
    public ErrorOr<Success> Delete()
    {
        this.SoftDelete();
        this.UpdateProperties();
        RegisterDomainEvent(new ServiceDeleteEvent(this));
        return Result.Success;
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel;

public abstract class Entity<TID> : Entity
{
    public TID ID { get; private set; }
}
public abstract class Entity
{
    protected Entity() { }

    private List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    [NotMapped]
    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void RegisterDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void SoftDelete()
    {
        this.Deleted = true;
    }
    protected void Deactive()
    {
        this.Active = false;
    }
    protected void Activated()
    {
        this.Active = true;
    }
    protected void Create(int user, int time)
    {
        this.Created_By = user;
        this.Created_At = time;
    }
    protected void Update(int user, int time)
    {
        this.Updated_By = user;
        this.Updated_At = time;
    }
    public bool Deleted { get; private set; }
    public bool Active { get; private set; }
    public int? Created_By { get; private set; }
    public int? Created_At { get; private set; }
    public int? Updated_By { get; private set; }
    public int? Updated_At { get; private set; }
}
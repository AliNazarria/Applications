using Applications.Domain.Application;
using Applications.Domain.Service;
using Common.Domain;
using Common.Usecase.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Applications.Infrastructure.Persist;

public class AppDbContext : DbContext
{
    private IDomainEventDispatcher? Dispatcher { get; }
    public IUserContextProvider UserContext { get; }
    public IDateTimeProvider DateTime { get; }
    public bool CurrentIsAdmin => UserContext.IsAdmin;

    public AppDbContext(DbContextOptions<AppDbContext> options,
        IDomainEventDispatcher? dispatcher,
        IDateTimeProvider dateTime,
        IUserContextProvider userContext)
        : base(options)
    {
        Dispatcher = dispatcher;
        UserContext = userContext;
        DateTime = dateTime;
    }
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<ApplicationService> ApplicationServices => Set<ApplicationService>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Application>().HasQueryFilter(x => CurrentIsAdmin
            || (x.Deleted == false && x.Active == true));
        modelBuilder.Entity<ApplicationService>().HasQueryFilter(x => CurrentIsAdmin
            || (x.Deleted == false && x.Active == true));
        modelBuilder.Entity<Service>().HasQueryFilter(x => CurrentIsAdmin
            || (x.Deleted == false && x.Active == true));
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Create(UserContext.UserID, DateTime.NowTimeStampInSeconds());
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.Created_At).IsModified = false;
                entry.Property(x => x.Created_By).IsModified = false;
                if (entry.Entity.PropertiesWasUpdated)
                    entry.Entity.Update(UserContext.UserID, DateTime.NowTimeStampInSeconds());
            }
        }

        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (Dispatcher == null) 
            return result;

        var entitiesWithEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await Dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }
    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
using Common.Domain;
using Common.Usecase.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Applications.Infrastructure.Persist;

public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher? _dispatcher;

    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher? dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
    }
    public DbSet<Common.Domain.Entities.Application> Applications => Set<Common.Domain.Entities.Application>();
    public DbSet<Common.Domain.Entities.ApplicationService> ApplicationServices => Set<Common.Domain.Entities.ApplicationService>();
    public DbSet<Domain.Service.Service> Services => Set<Domain.Service.Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (_dispatcher == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }
    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
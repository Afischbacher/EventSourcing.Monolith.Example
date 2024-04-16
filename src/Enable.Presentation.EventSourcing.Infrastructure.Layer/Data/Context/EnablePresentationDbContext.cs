using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;

public interface IEnablePresentationDbContext 
{
    DbSet<Event> Events { get; set; }

    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}

public class EnablePresentationDbContext : DbContext, IEnablePresentationDbContext
{
    public EnablePresentationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        DetectEventEntityChanges();
    }

    public bool HasEventsAdded { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        if (HasEventsAdded)
        {
            await EmitEvents();
            HasEventsAdded = false;
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (HasEventsAdded)
        {
            await EmitEvents();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        if (HasEventsAdded)
        {
            EmitEvents().RunSynchronously();
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private async Task EmitEvents() 
    {
    
    }

    public void DetectEventEntityChanges()
    {
        base.ChangeTracker.DetectingEntityChanges += (entity, detectedEntityChanges) =>
        {
            if (detectedEntityChanges.Entry.OriginalValues.EntityType.ClrType == typeof(Event) && detectedEntityChanges.Entry.State == EntityState.Added)
            {
                HasEventsAdded = true;
            }
        };
    }
}

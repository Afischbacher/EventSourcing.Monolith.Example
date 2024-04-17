using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;

/// <summary>
/// A database context example with event triggering 
/// </summary>
public interface IEnablePresentationDbContext
{
    DbSet<Event> Events { get; set; }

    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}

/// <summary>
/// A database context example with event triggering 
/// </summary>
public class EnablePresentationDbContext : DbContext, IEnablePresentationDbContext
{
    private readonly IServiceBusMessagingService serviceBusMessagingService;

    public EnablePresentationDbContext(DbContextOptions dbContextOptions, IServiceBusMessagingService serviceBusMessagingService) : base(dbContextOptions)
    {
        DetectEventEntityChanges();
        this.serviceBusMessagingService = serviceBusMessagingService;
    }

    public bool HasEventsAdded { get; set; }

    public DbSet<Event> Events { get; set; }

    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        if (HasEventsAdded)
        {
            await TriggerEvents();
            HasEventsAdded = false;
        }

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (HasEventsAdded)
        {
            await TriggerEvents();
            HasEventsAdded = false;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        if (HasEventsAdded)
        {
            TriggerEvents().RunSynchronously();
            HasEventsAdded = false;
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private async Task TriggerEvents() => await this.serviceBusMessagingService.SendMessageAsync("events");

    public void DetectEventEntityChanges() => base.ChangeTracker.DetectingEntityChanges += (entity, detectedEntityChanges) =>
                                                   {
                                                       if (detectedEntityChanges.Entry.OriginalValues.EntityType.ClrType == typeof(Event)
                                                            && detectedEntityChanges.Entry.State == EntityState.Added)
                                                       {
                                                           HasEventsAdded = true;
                                                       }
                                                   };
}

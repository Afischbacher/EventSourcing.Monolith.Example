
using Enable.Presentation.EventSourcing.DataAccess.Layer.Common.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;

/// <summary>
/// A repository for managing <see cref="Event"/> entities.
/// </summary>
public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event> GetBySequenceNumberAsync(long sequenceNumber);
}


/// <summary>
/// A repository for managing <see cref="Event"/> entities.
/// </summary>
public class EventsRepository(IEnablePresentationDbContext enablePresentationDbContext) : IBaseRepository<Event>, IEventRepository
{
    private readonly IEnablePresentationDbContext _enablePresentationDbContext = enablePresentationDbContext;

    public void Add(Event entity)
    {
        _enablePresentationDbContext.Events.Add(entity);
    }

    public async Task AddAsync(Event entity)
    {
        await _enablePresentationDbContext.Events.AddAsync(entity);
    }

    public void Delete(Event entity)
    {
        _enablePresentationDbContext.Events.Remove(entity);
    }

    public async Task<Event> GetBySequenceNumberAsync(long sequenceNumber)
    {
        return await _enablePresentationDbContext.Events.SingleAsync(e => e.SequenceNumber == sequenceNumber);
    }

    public void Save()
    {
        _enablePresentationDbContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _enablePresentationDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Update(Event entity)
    {
        _enablePresentationDbContext.Events.Update(entity);
    }
}

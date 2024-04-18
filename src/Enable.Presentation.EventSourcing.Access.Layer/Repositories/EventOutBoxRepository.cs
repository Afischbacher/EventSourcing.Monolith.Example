using Enable.Presentation.EventSourcing.DataAccess.Layer.Common.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;

namespace Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;

/// <summary>
/// A repository for managing <see cref="Event"/> entities.
/// </summary>
public interface IEventOutBoxRepository : IBaseRepository<EventOutBox>
{
}


/// <summary>
/// A repository for managing <see cref="Event"/> entities.
/// </summary>
public class EventOutBoxRepository(IEnablePresentationDbContext enablePresentationDbContext) : IEventOutBoxRepository, IBaseRepository<EventOutBox>
{
    private readonly IEnablePresentationDbContext _enablePresentationDbContext = enablePresentationDbContext;

    public void Add(EventOutBox entity)
    {
        _enablePresentationDbContext.EventOutBox.Add(entity);
    }

    public async Task AddAsync(EventOutBox entity)
    {
        await _enablePresentationDbContext.EventOutBox.AddAsync(entity);
    }

    public void Delete(EventOutBox entity)
    {
        _enablePresentationDbContext.EventOutBox.Remove(entity);
    }

    public void Save()
    {
        _enablePresentationDbContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _enablePresentationDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Update(EventOutBox entity)
    {
        _enablePresentationDbContext.EventOutBox.Update(entity);
    }
}

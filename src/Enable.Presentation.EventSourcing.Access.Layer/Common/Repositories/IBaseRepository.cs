using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;

namespace Enable.Presentation.EventSourcing.DataAccess.Layer.Common.Repositories;

/// <summary>
/// A generic interface for the Base Repository for the Data Access Layer
/// </summary>
/// <typeparam name="T">The entity from the context</typeparam>
public interface IBaseRepository<T> where T : class, IEntity
{
    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    void Save();

    Task AddAsync(T entity);

    Task SaveAsync(CancellationToken cancellationToken = default);

}

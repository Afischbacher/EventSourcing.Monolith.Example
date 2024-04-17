using Enable.Presentation.EventSourcing.DataAccess.Layer.Common.Repositories;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Context;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Enable.Presentation.EventSourcing.DataAccess.Layer.Repositories;

/// <summary>
/// A repository for managing <see cref="User"/> entities.
/// </summary>
public interface IUsersRepository : IBaseRepository<User>
{
    Task<User?> GetByUserIdAsync(Guid userId);
}

/// <summary>
/// A repository for managing <see cref="User"/> entities.
/// </summary>
public class UsersRepository(IEnablePresentationDbContext enablePresentationDbContext) : IUsersRepository
{
    private readonly IEnablePresentationDbContext _enablePresentationDbContext = enablePresentationDbContext;

    public void Add(User entity)
    {
        _enablePresentationDbContext.Users.Add(entity);
    }

    public async Task AddAsync(User entity)
    {
        await _enablePresentationDbContext.Users.AddAsync(entity);
    }

    public void Delete(User entity)
    {
        _enablePresentationDbContext.Users.Remove(entity);
    }

    public async Task<User?> GetByUserIdAsync(Guid userId)
    {
        return await _enablePresentationDbContext.Users.SingleOrDefaultAsync(e => e.Id == userId);
    }

    public void Save()
    {
        _enablePresentationDbContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _enablePresentationDbContext.SaveChangesAsync(cancellationToken);
    }

    public void Update(User entity)
    {
        _enablePresentationDbContext.Users.Update(entity);
    }
}

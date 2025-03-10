using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace UrlShortener.DataAccess.Repository;

public class Repository<T> where T : class
{
    protected readonly ISession _session;

    public Repository(ISession session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _session.GetAsync<T>(id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _session.Query<T>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _session.Query<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.SaveAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.UpdateAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        using (var transaction = _session.BeginTransaction())
        {
            await _session.DeleteAsync(entity, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _session.Query<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return await _session.Query<T>().CountAsync(cancellationToken);
    }
}
using NHibernate;
using NHibernate.Linq;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Repository;

public class UrlRepository : Repository<URL>, IUrlRepository
{
    public UrlRepository(ISession session) : base(session)
    {
    }

    public async Task<URL> GetByShortUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        return await _session.Query<URL>()
            .FirstOrDefaultAsync(u => u.ShortURL == shortUrl, cancellationToken);
    }

    public async Task<bool> IsShortUrlUniqueAsync(string shortURL, CancellationToken cancellationToken)
    {
        bool exists = await _session.Query<URL>()
            .AnyAsync(u => u.ShortURL == shortURL, cancellationToken)
            .ConfigureAwait(false);

        return !exists;
    }
}

using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Repository;

public interface IUrlRepository : IRepository<URL>
{
    Task<URL> GetByShortUrlAsync(string ShortURL, CancellationToken cancellationToken);
    Task<bool> IsShortUrlUniqueAsync(string shortCode, CancellationToken cancellationToken);
}

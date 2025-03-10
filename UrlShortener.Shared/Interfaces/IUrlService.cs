using UrlShortener.Shared.DTOs;

namespace UrlShortener.Shared.Interfaces;

public interface IUrlService
{
    Task<UrlDto> CreateUrlAsync(string originalUrl, CancellationToken cancellationToken);
    Task<String> GetUrlByShortUrlAsync(string shortCode, CancellationToken cancellationToken);
    Task<PaginatedResult<UrlDto>> GetAllURLsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<UrlDto> UpdateUrlAsync(UrlDto urlDto, CancellationToken cancellationToken);
    Task DeleteUrlAsync(int id, CancellationToken cancellationToken);
    Task<UrlDto> GetUrlByIdAsync(int id, CancellationToken cancellationToken);
}

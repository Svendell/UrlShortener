namespace UrlShortener.Shared.Interfaces;

public interface IUrlShorteningService
{
    Task<string> GenerateShortURL(int length = 8, CancellationToken cancellationToken = default);
}

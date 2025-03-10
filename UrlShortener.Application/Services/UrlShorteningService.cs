using System.Security.Cryptography;
using System.Text;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Shared.Exceptions;
using UrlShortener.Shared.Interfaces;

public class UrlShorteningService(IUrlRepository urlRepository) : IUrlShorteningService
{
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public async Task<string> GenerateShortURL(int length = 8, CancellationToken cancellationToken = default)
    {
        const int maxAttempts = 10;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var shortUrl = GenerateRandomString(length);
            bool exists = await urlRepository.IsShortUrlUniqueAsync(shortUrl, cancellationToken);
            if (exists)
            {
                return shortUrl;
            }
        }
        throw new BusinessException("SHORT_URL_GENERATION_FAILED", "Failed to generate a unique short URL after multiple attempts.");
    }

    private string GenerateRandomString(int length)
    {
        var sb = new StringBuilder(length);
        byte[] randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        for (int i = 0; i < length; i++)
        {
            int index = randomBytes[i] % Characters.Length;
            sb.Append(Characters[index]);
        }
        return sb.ToString();
    }
}
using AutoMapper;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Domain.Entities;
using UrlShortener.Shared.DTOs;
using UrlShortener.Shared.Exceptions;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.Services;

public class UrlService(
    IUrlShorteningService urlShorteningService,
    IUrlRepository urlRepository,
    IMapper mapper
) : IUrlService
{
    private const int CLOCK_COUNT_DEFAULT = 0;

    public async Task<UrlDto> CreateUrlAsync(string originalUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(originalUrl))
        {
            throw new BusinessException("URL_EMPTY", "Original URL cannot be empty.");
        }

        URL newURL = new URL
        {
            OriginalURL = originalUrl,
            ShortURL = await urlShorteningService.GenerateShortURL(8, cancellationToken),
            CreatedAt = DateTime.UtcNow,
            ClickCount = CLOCK_COUNT_DEFAULT
        };

        var existingUrl = await urlRepository.FindAsync(u => u.OriginalURL == originalUrl, cancellationToken);
        if (existingUrl.Any())
        {
            throw new BusinessException("URL_EXISTS", "The original URL already exists.");
        }

        await urlRepository.AddAsync(newURL, cancellationToken);

        return mapper.Map<UrlDto>(newURL);
    }

    public async Task DeleteUrlAsync(int id, CancellationToken cancellationToken)
    {
        var url = await urlRepository.GetByIdAsync(id, cancellationToken);
        if (url == null)
        {
            throw new BusinessException("URL_NOT_FOUND", $"URL with ID {id} not found.");
        }

        await urlRepository.DeleteAsync(url, cancellationToken);
    }

    public async Task<PaginatedResult<UrlDto>> GetAllURLsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {

        var totalCount = await urlRepository.GetTotalCountAsync(cancellationToken);
        var urls = await urlRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);

        var urlDtos = mapper.Map<List<UrlDto>>(urls);

        return new PaginatedResult<UrlDto>
        {
            Items = urlDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<String> GetUrlByShortUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(shortUrl))
        {
            throw new BusinessException("SHORT_URL_EMPTY", "Short URL cannot be empty.");
        }

        var url = await urlRepository.GetByShortUrlAsync(shortUrl, cancellationToken);
        if (url is null)
        {
            throw new BusinessException("SHORT_URL_NOT_FOUND", $"Short URL '{shortUrl}' not found.");
        }

        url.ClickCount++;
        await urlRepository.UpdateAsync(url, cancellationToken);

        return Uri.EscapeUriString(url.OriginalURL);
    }

    public async Task<UrlDto> UpdateUrlAsync(UrlDto urlDto, CancellationToken cancellationToken)
    {
        if (urlDto is null)
        {
            throw new BusinessException("URL_DTO_NULL", "URL DTO cannot be null.");
        }

        if (urlDto.Id <= 0)
        {
            throw new BusinessException("INVALID_URL_ID", "URL DTO must contain a valid ID.");
        }

        var existingUrl = await urlRepository.GetByIdAsync(urlDto.Id, cancellationToken);
        if (existingUrl is null)
        {
            throw new BusinessException("URL_NOT_FOUND", $"URL with ID {urlDto.Id} not found.");
        }

        var updatedUser = mapper.Map(urlDto, existingUrl);

        await urlRepository.UpdateAsync(updatedUser, cancellationToken);

        return mapper.Map<UrlDto>(existingUrl);
    }

    public async Task<UrlDto> GetUrlByIdAsync(int id, CancellationToken cancellationToken)
    {
        var url = await urlRepository.GetByIdAsync(id, cancellationToken);

        if (url is null)
        {
            throw new BusinessException("URL_NOT_FOUND", $"URL with ID {id} not found.");
        }

        return mapper.Map<UrlDto>(url);
    }
}

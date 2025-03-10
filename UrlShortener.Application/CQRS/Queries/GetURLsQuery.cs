using MediatR;
using UrlShortener.Shared.DTOs;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Queries;

public class GetURLsQuery : IRequest<PaginatedResult<UrlDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetURLsQueryHandler(
    IUrlService urlService
) : IRequestHandler<GetURLsQuery, PaginatedResult<UrlDto>>
{
    async Task<PaginatedResult<UrlDto>> IRequestHandler<GetURLsQuery, PaginatedResult<UrlDto>>.Handle(GetURLsQuery request, CancellationToken cancellationToken)
    {
        return await urlService.GetAllURLsAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
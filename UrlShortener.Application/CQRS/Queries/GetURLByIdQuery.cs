using MediatR;
using UrlShortener.Shared.DTOs;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Queries;

public class GetURLByIdQuery : IRequest<UrlDto>
{
    public int Id { get; set; }
}

public class GetURLByIdQueryHandler(
    IUrlService urlService
) : IRequestHandler<GetURLByIdQuery, UrlDto>
{
    public async Task<UrlDto> Handle(GetURLByIdQuery request, CancellationToken cancellationToken)
    {
        return await urlService.GetUrlByIdAsync(request.Id, cancellationToken);
    }
}
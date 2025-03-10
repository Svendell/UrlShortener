using MediatR;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Queries;

public class GetURLByShortUrlQuery : IRequest<String>
{
    public string ShortURL { get; set; }
}

public class GetURLByShortUrlQueryHandler(
    IUrlService urlService
) : IRequestHandler<GetURLByShortUrlQuery, String>
{
    public async Task<String> Handle(GetURLByShortUrlQuery request, CancellationToken cancellationToken)
    {
        return await urlService.GetUrlByShortUrlAsync(request.ShortURL, cancellationToken);
    }
}
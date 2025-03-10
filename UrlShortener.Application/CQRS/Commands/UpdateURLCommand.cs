using MediatR;
using UrlShortener.Shared.DTOs;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Commands;

public class UpdateURLCommand : IRequest<UrlDto>
{
    public UrlDto UrlDto { get; set; }
}

public class UpdateURLCommandHandler(
    IUrlService urlService
) : IRequestHandler<UpdateURLCommand, UrlDto>
{
    public async Task<UrlDto> Handle(UpdateURLCommand request, CancellationToken cancellationToken)
    {
        var updatedURL = await urlService.UpdateUrlAsync(request.UrlDto, cancellationToken);
        return updatedURL;
    }
}
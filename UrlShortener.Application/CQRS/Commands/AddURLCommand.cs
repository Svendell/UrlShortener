using MediatR;
using System.ComponentModel.DataAnnotations;
using UrlShortener.Shared.DTOs;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Commands;

public class AddURLCommand : IRequest<UrlDto>
{
    [Url]
    [Required]
    public string OriginalURL { get; set; }
}

public class AddURLCommandHandler(
    IUrlService urlService
) : IRequestHandler<AddURLCommand, UrlDto>
{
    public async Task<UrlDto> Handle(AddURLCommand request, CancellationToken cancellationToken)
    {
        var newURL = await urlService.CreateUrlAsync(request.OriginalURL, cancellationToken);
        return newURL;
    }
}
using MediatR;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.Application.CQRS.Commands;

public class DeleteURLCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteURLCommandHandler(
    IUrlService urlService
) : IRequestHandler<DeleteURLCommand>
{
    public async Task Handle(DeleteURLCommand request, CancellationToken cancellationToken)
    {
        await urlService.DeleteUrlAsync(request.Id, cancellationToken);
    }
}
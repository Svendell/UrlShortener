using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.CQRS.Commands;
using UrlShortener.Application.CQRS.Queries;

namespace UrlShortener.Controllers;

public class HomeController(IMediator mediator) : Controller
{
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetURLsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var paginatedResult = await mediator.Send(query, cancellationToken);
        return View(paginatedResult);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AddURLCommand());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddURLCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var query = new GetURLByIdQuery { Id = id };
        var urlDto = await mediator.Send(query, cancellationToken);

        return View(new UpdateURLCommand { UrlDto = urlDto });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateURLCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteURLCommand { Id = id };
        await mediator.Send(command, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> RedirectToOriginal(string shortURL, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(shortURL))
        {
            return NotFound();
        }

        var query = new GetURLByShortUrlQuery { ShortURL = shortURL };
        var url = await mediator.Send(query, cancellationToken);


        return Redirect(url);
    }

    [HttpGet]
    public IActionResult Error(string errorCode, string message)
    {
        ViewBag.ErrorCode = errorCode ?? "UNKNOWN_ERROR";
        ViewBag.Message = message ?? "An unknown error occurred.";
        return View();
    }
}
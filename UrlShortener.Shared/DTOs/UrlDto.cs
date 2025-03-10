using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Shared.DTOs;

public class UrlDto
{
    public int Id { get; set; }
    [Url]
    public string OriginalURL { get; set; }
    public string ShortURL { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClickCount { get; set; }
}
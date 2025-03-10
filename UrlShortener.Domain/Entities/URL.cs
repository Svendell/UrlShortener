namespace UrlShortener.Domain.Entities;

public class URL
{
    public virtual int Id { get; set; }
    public virtual string OriginalURL { get; set; }
    public virtual string ShortURL { get; set; }
    public virtual DateTime CreatedAt { get; set; }
    public virtual int ClickCount { get; set; }
}

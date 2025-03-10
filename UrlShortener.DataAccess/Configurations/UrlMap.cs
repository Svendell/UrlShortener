using FluentNHibernate.Mapping;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Mappings;

public class UrlMap : ClassMap<URL>
{
    public UrlMap()
    {
        Table("URLs");
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.OriginalURL).Not.Nullable();
        Map(x => x.ShortURL).Not.Nullable().Unique();
        Map(x => x.CreatedAt).Not.Nullable();
        Map(x => x.ClickCount).Not.Nullable().Default("0");
    }
}
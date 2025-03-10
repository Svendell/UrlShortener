using UrlShortener.Application.CQRS.Queries;
using UrlShortener.Application.Mapping;
using UrlShortener.Application.Services;
using UrlShortener.DataAccess;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Shared.Interfaces;

namespace UrlShortener.API.Extensions;

public static class ServiceCollectionExtensions 
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetURLsQueryHandler).Assembly));
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), typeof(Program).Assembly);

        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IUrlRepository, UrlRepository>();
        services.AddScoped<IUrlShorteningService, UrlShorteningService>();

        services.AddSingleton<NHibernateHelper>();

        services.AddScoped(provider =>
        {
            var nhibernateHelper = provider.GetRequiredService<NHibernateHelper>();
            return nhibernateHelper.OpenSession();
        });

        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalValidationFilter>();
        });
    }
}

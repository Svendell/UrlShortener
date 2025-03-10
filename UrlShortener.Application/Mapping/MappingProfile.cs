using AutoMapper;
using UrlShortener.Domain.Entities;
using UrlShortener.Shared.DTOs;

namespace UrlShortener.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<URL, UrlDto>();
        CreateMap<UrlDto, URL>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
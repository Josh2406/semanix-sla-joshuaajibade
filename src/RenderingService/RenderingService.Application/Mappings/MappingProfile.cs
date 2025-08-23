using AutoMapper;
using RenderingService.Domain.Entities;
using RenderingService.Domain.Models;
using System.Text.Json;

namespace RenderingService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RenderedForm, RenderedFormDto>()
                .ForMember(x => x.JsonPayload, opt => opt.MapFrom(x =>
                    JsonSerializer.Deserialize<object>(x.JsonPayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })))
                .ReverseMap();
        }
    }
}

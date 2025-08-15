using AutoMapper;
using FormsService.Application.Models.Response;
using FormsService.Domain.Entities;
using Shared.Common.Events;

namespace FormsService.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Form, FormUpdatedEvent>()
                .ForMember(x => x.FormId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<Form, FormPublishedEvent>()
                .ForMember(x => x.FormId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<Form, FormDto>().ReverseMap();
        }
    }
}

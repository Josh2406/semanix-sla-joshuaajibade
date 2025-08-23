namespace FormsService.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<Form, FormUpdatedEvent>()
                .ForMember(x => x.UpdatedFormId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<Form, FormPublishedEvent>()
                .ForMember(x => x.PublishedFormId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();

            CreateMap<Form, FormDto>().ReverseMap();
        }
    }
}

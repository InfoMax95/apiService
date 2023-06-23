using apiService.DTO;
using apiService.Models;
using AutoMapper;

namespace apiService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Authors.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Typologies.Name));
        }
    }
}

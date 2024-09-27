using API.Dtos;
using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<User, UserDto>()
             .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<UserDto, User>()
            .ForMember(dest => dest.Role, opt => opt.Ignore()) 
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore());


            CreateMap<WarehouseCreateDto, Warehouse>() //insert
           .ForMember(dest => dest.City, opt => opt.Ignore())
           .ForMember(dest => dest.Country, opt => opt.Ignore());

            // to show items from api to the dto items to show 
            CreateMap<Item, ItemDto>();

        }
    }
}

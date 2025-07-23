using AutoMapper;
using FoodOrdering.API.Models;
using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItom, MenuItomResponseDto>()
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.FirstName + " " + src.Vendor.LastName));

            CreateMap<CreateMenuItomDto, MenuItom>();

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.FirstName + " " + src.Vendor.LastName));

            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.MenuItomName, opt => opt.MapFrom(src => src.MenuItom.Name));
        }
    }
}
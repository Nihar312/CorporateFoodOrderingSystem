using AutoMapper;
using FoodOrdering.API.Models;
using FoodOrdering.API.DTOs;

namespace FoodOrdering.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Building mappings
            CreateMap<Building, BuildingResponseDto>();
            CreateMap<CreateBuildingDto, Building>();

            // Canteen mappings
            CreateMap<Canteen, CanteenResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name));
            CreateMap<CreateCanteenDto, Canteen>();

            // MenuItem mappings
            CreateMap<MenuItem, MenuItemResponseDto>()
                .ForMember(dest => dest.CanteenName, opt => opt.MapFrom(src => src.Canteen.Name))
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.FirstName + " " + src.Vendor.LastName));
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<CreateMenuItemDto, MenuItem>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (MenuCategory)src.Category));

            // Order mappings
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.FirstName + " " + src.Vendor.LastName));

            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name));
        }
    }
}
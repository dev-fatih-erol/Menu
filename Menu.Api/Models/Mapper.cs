using System;
using AutoMapper;
using Menu.Core.Models;

namespace Menu.Api.Models
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<City, CityDto>();

            CreateMap<Venue, VenueDto>();

            CreateMap<Category, CategoryDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => string.Format("{0:N2}", s.Price)))
                .ForMember(d => d.StartTime, o => o.MapFrom(s => new TimeSpan(s.StartTime)))
                .ForMember(d => d.EndTime, o => o.MapFrom(s => new TimeSpan(s.EndTime)))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => IsAvailable(s)));

            CreateMap<Option, OptionDto>();

            CreateMap<OptionItem, OptionItemDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => string.Format("{0:N2}", s.Price)));

            CreateMap<Option, OptionOptionItemDto>()
                .ForMember(d => d.OptionItems, o => o.MapFrom(s => s.OptionItem));

            CreateMap<Category, CategoryProductDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Product));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => string.Format("{0:N2}", s.Price)))
                .ForMember(d => d.StartTime, o => o.MapFrom(s => new TimeSpan(s.StartTime)))
                .ForMember(d => d.EndTime, o => o.MapFrom(s => new TimeSpan(s.EndTime)))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => IsAvailable(s)))
                .ForMember(d => d.Options, o => o.MapFrom(s => s.Option));
        }

        private bool IsAvailable(Product product)
        {
            var startTime = new TimeSpan(product.StartTime);

            var endTime = new TimeSpan(product.EndTime);

            var currentTime = DateTime.Now.TimeOfDay;

            if ((currentTime >= startTime) && (currentTime <= endTime))
            {
                return true;
            }

            return false;
        }
    }
}
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

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)));

            CreateMap<Category, CategoryDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Price, o => o.MapFrom(s => string.Format("{0:N2}", s.Price)))
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
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
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => IsAvailable(s)))
                .ForMember(d => d.Options, o => o.MapFrom(s => s.Option));
        }

        private bool IsAvailable(Product product)
        {
            var openingTime = new TimeSpan(product.OpeningTime);

            var closingTime = new TimeSpan(product.ClosingTime);

            var currentTime = DateTime.Now.TimeOfDay;

            if ((currentTime >= openingTime) && (currentTime <= closingTime))
            {
                return true;
            }

            return false;
        }
    }
}
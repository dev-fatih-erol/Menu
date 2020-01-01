﻿using System;
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

            CreateMap<Venue, RandomVenueDto>()
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
                .ForMember(d => d.Address, o => o.MapFrom(s => "Yalı, Turgut Özal Blv. No:189, 34844 Maltepe/İstanbul"))
                .ForMember(d => d.Rate, o => o.MapFrom(s => 4))
                .ForMember(d => d.CommentCount, o => o.MapFrom(s => 287));

            CreateMap<Venue, VenueDetailDto>()
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
                .ForMember(d => d.Address, o => o.MapFrom(s => "Yalı, Turgut Özal Blv. No:189, 34844 Maltepe/İstanbul"))
                .ForMember(d => d.Rate, o => o.MapFrom(s => 4))
                .ForMember(d => d.CommentCount, o => o.MapFrom(s => 287))
                .ForMember(d => d.Speed, o => o.MapFrom(s => 7))
                .ForMember(d => d.Waiter, o => o.MapFrom(s => 9))
                .ForMember(d => d.Flavor, o => o.MapFrom(s => 8));

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
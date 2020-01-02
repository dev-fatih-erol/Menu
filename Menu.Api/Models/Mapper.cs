using System;
using System.Linq;
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

            CreateMap<CommentRating, CommentRatingDto>();

            CreateMap<Option, OptionOptionItemDto>()
                .ForMember(d => d.OptionItems, o => o.MapFrom(s => s.OptionItem));

            CreateMap<Category, CategoryProductDto>()
                .ForMember(d => d.Products, o => o.MapFrom(s => s.Product));

            CreateMap<Venue, RandomVenueDto>()
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
                .ForMember(d => d.Rating, o => o.MapFrom(s => CalculateRating(s)))
                .ForMember(d => d.CommentCount, o => o.MapFrom(s => s.CommentRating.Count()));

            CreateMap<Venue, VenueDetailDto>()
                .ForMember(d => d.OpeningTime, o => o.MapFrom(s => new TimeSpan(s.OpeningTime)))
                .ForMember(d => d.ClosingTime, o => o.MapFrom(s => new TimeSpan(s.ClosingTime)))
                .ForMember(d => d.Rating, o => o.MapFrom(s => CalculateRating(s)))
                .ForMember(d => d.CommentCount, o => o.MapFrom(s => s.CommentRating.Count()))
                .ForMember(d => d.Speed, o => o.MapFrom(s => CalculateSpeed(s)))
                .ForMember(d => d.Waiter, o => o.MapFrom(s => CalculateWaiter(s)))
                .ForMember(d => d.Flavor, o => o.MapFrom(s => CalculateFlavor(s)));

            CreateMap<User, UserProfileDto>();

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

        private int CalculateRating(Venue venue)
        {
            return venue.CommentRating.Any() ?
                venue.CommentRating.Sum(r => r.Speed + r.Waiter + r.Flavor) / venue.CommentRating.Count() / 3 : 0;
        }

        private int CalculateSpeed(Venue venue)
        {
            return venue.CommentRating.Any() ?
                venue.CommentRating.Sum(r => r.Speed) / venue.CommentRating.Count() : 0;
        }

        private int CalculateWaiter(Venue venue)
        {
            return venue.CommentRating.Any() ?
                venue.CommentRating.Sum(r => r.Waiter) / venue.CommentRating.Count() : 0;
        }

        private int CalculateFlavor(Venue venue)
        {
            return venue.CommentRating.Any() ?
                venue.CommentRating.Sum(r => r.Flavor) / venue.CommentRating.Count() : 0;
        }
    }
}
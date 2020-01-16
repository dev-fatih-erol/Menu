using System;
using Menu.Core.Enums;
using Menu.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Menu.Data
{
    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions<MenuContext> options)
            : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<Venue> Venues { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<OptionItem> OptionItems { get; set; }

        public DbSet<CommentRating> CommentRatings { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .ToTable("City");

            modelBuilder.Entity<Venue>()
                .ToTable("Venue");

            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Product>()
                .ToTable("Product");

            modelBuilder.Entity<Option>()
                .ToTable("Option");

            modelBuilder.Entity<OptionItem>()
                .ToTable("OptionItem");

            modelBuilder.Entity<CommentRating>()
                .ToTable("CommentRating");

            modelBuilder.Entity<Favorite>()
                .ToTable("Favorite");

            modelBuilder.Entity<Order>()
                .ToTable("Order");

            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetail");

            modelBuilder.Entity<Table>()
                .ToTable("Table");

            modelBuilder.Entity<Venue>()
                .Property(v => v.VenueType)
                .HasConversion(v => v.ToString(),
                               v => (VenueType)Enum.Parse(typeof(VenueType),
                               v));

            modelBuilder.Entity<Option>()
                .Property(o => o.OptionType)
                .HasConversion(o => o.ToString(),
                               o => (OptionType)Enum.Parse(typeof(OptionType),
                               o));
        }
    }
}
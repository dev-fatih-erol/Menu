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

        public DbSet<AppAbout> AppAbouts { get; set; }

        public DbSet<AppSlider> AppSliders { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Venue> Venues { get; set; }

        public DbSet<Cash> Cashes { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<Kitchen> Kitchens { get; set; }

        public DbSet<Waiter> Waiters { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<TableWaiter> TableWaiters { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<OptionItem> OptionItems { get; set; }

        public DbSet<OrderTable> OrderTables { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderWaiter> OrderWaiters { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderPayment> OrderPayments { get; set; }

        public DbSet<OrderCash> OrderCashes { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<VenuePaymentMethod> VenuePaymentMethods { get; set; }

        public DbSet<CommentRating> CommentRatings { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<SuggestionComplaint> SuggestionComplaints { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<VenueFeature> VenueFeatures { get; set; }

        public DbSet<WaiterToken> WaiterTokens { get; set; }

        public DbSet<DayReports> DayReportss { get; set; }

        public DbSet<NotificationWaiterSubject> NotificationWaiterSubjects { get; set; }

        public DbSet<NotificationWaiter> NotificationWaiters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppAbout>()
                .ToTable("AppAbout");

            modelBuilder.Entity<AppSlider>()
                .ToTable("AppSlider");

            modelBuilder.Entity<City>()
                .ToTable("City");

            modelBuilder.Entity<Manager>()
               .ToTable("Manager");

            modelBuilder.Entity<Venue>()
                .ToTable("Venue");

            modelBuilder.Entity<Cash>()
                .ToTable("Cash");

            modelBuilder.Entity<Kitchen>()
                .ToTable("Kitchen");

            modelBuilder.Entity<Waiter>()
                .ToTable("Waiter");

            modelBuilder.Entity<Table>()
                .ToTable("Table");

            modelBuilder.Entity<TableWaiter>()
                .ToTable("TableWaiter");

            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Product>()
                .ToTable("Product");

            modelBuilder.Entity<Option>()
                .ToTable("Option")
                .HasMany(x => x.OptionItem)
                .WithOne(x => x.Option)
                .OnDelete(DeleteBehavior.Cascade);     

            modelBuilder.Entity<OptionItem>()
                .ToTable("OptionItem");

            modelBuilder.Entity<UserToken>()
                .ToTable("UserToken");

            modelBuilder.Entity<OrderTable>()
                .ToTable("OrderTable");

            modelBuilder.Entity<Order>()
                .ToTable("Order");

            modelBuilder.Entity<OrderWaiter>()
                .ToTable("OrderWaiter");

            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetail");

            modelBuilder.Entity<OrderPayment>()
                .ToTable("OrderPayment");

            modelBuilder.Entity<OrderCash>()
                .ToTable("OrderCash");

            modelBuilder.Entity<PaymentMethod>()
                .ToTable("PaymentMethod");

            modelBuilder.Entity<VenuePaymentMethod>()
                .ToTable("VenuePaymentMethod");

            modelBuilder.Entity<CommentRating>()
                .ToTable("CommentRating");

            modelBuilder.Entity<Favorite>()
                .ToTable("Favorite");

            modelBuilder.Entity<SuggestionComplaint>()
                .ToTable("SuggestionComplaint");

            modelBuilder.Entity<Feature>()
                .ToTable("Feature");

            modelBuilder.Entity<VenueFeature>()
                .ToTable("VenueFeature");

            modelBuilder.Entity<WaiterToken>()
                .ToTable("WaiterToken");

            modelBuilder.Entity<DayReports>()
                .ToTable("DayReports");

            modelBuilder.Entity<NotificationWaiterSubject>()
                .ToTable("NotificationWaiterSubject");

            modelBuilder.Entity<NotificationWaiter>()
                .ToTable("NotificationWaiter");

            modelBuilder.Entity<Venue>()
                .Property(v => v.VenueType)
                .HasConversion(v => v.ToString(),
                               v => (VenueType)Enum.Parse(typeof(VenueType),
                               v));

            modelBuilder.Entity<Table>()
                .Property(t => t.TableStatus)
                .HasConversion(t => t.ToString(),
                               t => (TableStatus)Enum.Parse(typeof(TableStatus),
                               t));

            modelBuilder.Entity<Option>()
                .Property(o => o.OptionType)
                .HasConversion(o => o.ToString(),
                               o => (OptionType)Enum.Parse(typeof(OptionType),
                               o));

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasConversion(o => o.ToString(),
                               o => (OrderStatus)Enum.Parse(typeof(OrderStatus),
                               o));

            modelBuilder.Entity<OrderCash>()
                        .Property(o => o.OrderCashStatus)
                        .HasConversion(o => o.ToString(),
                                       o => (OrderCashStatus)Enum.Parse(typeof(OrderCashStatus),
                                       o));

            modelBuilder.Entity<SuggestionComplaint>()
                .Property(s => s.SuggestionComplaintStatus)
                .HasConversion(s => s.ToString(),
                               s => (SuggestionComplaintStatus)Enum.Parse(typeof(SuggestionComplaintStatus),
                               s));

            modelBuilder.Entity<SuggestionComplaint>()
                .Property(s => s.SubjectType)
                .HasConversion(s => s.ToString(),
                               s => (SubjectType)Enum.Parse(typeof(SubjectType),
                               s));
        }
    }
}
using DomainLayer.Models;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Banners;
using DomainLayer.Models.DiscountCodes;
using DomainLayer.Models.FAQs;
using DomainLayer.Models.Newsletters;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Products;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Sliders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    #region Articles
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleGroup> ArticleGroups { get; set; }
    public DbSet<ArticleComment> ArticleComments { get; set; }
    #endregion

    #region Banners

    public DbSet<Banner> Banners { get; set; }

    #endregion

    #region DiscountCodes

    public DbSet<DiscountCode> DiscountCodes { get; set; }


    #endregion

    #region FAQs

    public DbSet<Faq> Faqs { get; set; }
    public DbSet<FaqDetail> FaqDetails { get; set; }

    #endregion

    #region Newsletters

    public DbSet<Newsletter> Newsletters { get; set; }
    public DbSet<NewsletterMember> NewsletterMembers { get; set; }

    #endregion

    #region Orders
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderAddress> OrderAddresses { get; set; }
    #endregion

    #region Products
    public DbSet<Product> Products { get; set; }
    public DbSet<AmazingProduct> AmazingProducts { get; set; }
    public DbSet<ProductComment> ProductComments { get; set; }
    public DbSet<ProductGroup> ProductGroups { get; set; }
    public DbSet<ProductSpecifications> ProductSpecifications { get; set; }
    #endregion

    #region Roles
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    #endregion

    #region Sliders

    public DbSet<ShopSlider> ShopSliders { get; set; }

    #endregion

    #region Tickets
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }
    #endregion

    #region Users
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<UserCard> UserCards { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    #endregion

    #region Wallets
    public DbSet<Wallet> Wallets { get; set; }
    #endregion

    public DbSet<SiteSetting> SiteSettings { get; set; }
    public DbSet<ShippingCost> ShippingCosts { get; set; }
    public DbSet<ContactUs> ContactUs { get; set; }
    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<SiteRules> SiteRules { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<PopUpModel> PopUpModel { get; set; }
    public DbSet<UserPoint> UserPoints { get; set; }

    public virtual DbCommand CreateCommand()
    {
        return Database.GetDbConnection().CreateCommand();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Apply Entities Mapping
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

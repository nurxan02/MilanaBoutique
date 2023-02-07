using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilanaBoutique.Models;

namespace MilanaBoutique.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }

        public DbSet<ProductSizeColor> ProductSizeColors { get; set; }

        public DbSet<ProductColor> ProductColors { get; set; }

        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Status> Statuses { get; set; }

        public DbSet<Settings> Settings { get; set; }

        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<BlacklistTokens> BlacklistTokens { get; set; }

        public DbSet<Questions> Questions { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag>  BlogTags { get; set; }

        public DbSet<AboutUs> AboutUs { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
    }
}

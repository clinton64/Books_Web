using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options
            ) : base(options)
        { 
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { set;get; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<OrderHeader> OrderHeaders { set; get; }
        public DbSet<OrderDetail > OrderDetails { set; get; }   
    }
}

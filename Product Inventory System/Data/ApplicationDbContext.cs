using Microsoft.EntityFrameworkCore;
using Product_Inventory_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Product_Inventory_System.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

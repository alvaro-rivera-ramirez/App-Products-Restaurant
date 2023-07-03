using Microsoft.EntityFrameworkCore;
using ApiRestaurant.Models;

namespace ApiRestaurant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Cliente> Clientes { get; set; }
    }
}

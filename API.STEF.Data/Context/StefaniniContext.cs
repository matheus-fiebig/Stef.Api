using API.STEF.Data.Context.Configurations;
using API.STEF.Domain.ProductAggregator;
using Microsoft.EntityFrameworkCore;

namespace API.STEF.Data.Context
{
    public class StefaniniContext : DbContext
    {
        public StefaniniContext(DbContextOptions<StefaniniContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {      
            optionsBuilder.LogTo(Console.WriteLine);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

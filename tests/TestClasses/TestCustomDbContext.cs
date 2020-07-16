using Microsoft.EntityFrameworkCore;
using TestClasses.Configuration;
using TestClasses.Entities;

namespace TestClasses
{
    public class TestCustomDbContext : DbContext
    {
        public TestCustomDbContext()
        {
        }

        public TestCustomDbContext(
            DbContextOptions<TestCustomDbContext> options,
            ConfigurationOptions configurationOptions)
            : base(options)
        {
            this.ConfigurationOptions = configurationOptions;
        }

        public ConfigurationOptions ConfigurationOptions { get; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}

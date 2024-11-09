using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Model;

namespace Infrastructure.Data
{
    public class BoughtItDbContext:IdentityDbContext<User,IdentityRole<int>,int>
    {
        public BoughtItDbContext(DbContextOptions<BoughtItDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users").ToTable(tb=>tb.HasTrigger("tr_UsersTimestamp"));
            modelBuilder.Entity<Order>().ToTable("Orders").ToTable(tb => tb.HasTrigger("tr_OrdersTimestamp"));
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Product)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(o => o.GlobalProductId)
                .HasPrincipalKey(p => p.GlobalProductId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(o => o.OrderId);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.Now;
                    entry.Property(e => e.CreatedDate).IsModified = false;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Company.Models
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()
        {
        }

        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Customers> Customers { get; set; }

        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>(entity =>
            {
                entity.Property(e => e.IsActive).HasColumnName("isActive").HasDefaultValue(true);
            });
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.Property(e => e.IsActive).HasColumnName("isActive").HasDefaultValue(true);
            });
        }
    }
}

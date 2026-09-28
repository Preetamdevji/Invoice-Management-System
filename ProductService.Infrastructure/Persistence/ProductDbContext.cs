using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<SpecialOffer> SpecialOffers => Set<SpecialOffer>();
        public DbSet<SpecialOfferProductRecord> SpecialOfferProducts => Set<SpecialOfferProductRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Production");
                entity.HasKey(e => e.ProductId);
                entity.Property(p => p.ProductId).HasColumnName("ProductID");
                entity.Property(p => p.Name).HasColumnName("Name");
                entity.Property(p => p.ProductNumber).HasColumnName("ProductNumber");
                entity.Property(p => p.Color).HasColumnName("Color");
                entity.Property(p => p.ListPrice).HasColumnName("ListPrice");
            });

            modelBuilder.Entity<SpecialOffer>(entity =>
            {
                entity.ToTable("SpecialOffer", "Sales");
                entity.HasKey(so => so.SpecialOfferId);
                entity.Property(so => so.SpecialOfferId).HasColumnName("SpecialOfferID");
                entity.Property(so => so.Description).HasColumnName("Description");
                entity.Property(so => so.DiscountPct).HasColumnName("DiscountPct");
            });

            modelBuilder.Entity<SpecialOfferProductRecord>(entity =>
            {
                entity.ToTable("SpecialOfferProduct", "Sales");
                //composite key for junction table
                entity.HasKey(sop => new { sop.SpecialOfferId, sop.ProductId });
                entity.Property(sop => sop.SpecialOfferId).HasColumnName("SpecialOfferID");
                entity.Property(sop => sop.ProductId).HasColumnName("ProductID");
            });
        }
    }
}

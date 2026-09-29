using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Persistence
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            :base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<PersonRecord> People => Set<PersonRecord>();
        public DbSet<AddressRecord> Addresses => Set<AddressRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "dbo");
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.CustomerId)
                .HasColumnName("CustomerID");
                entity.Property(c => c.PersonId)
                .HasColumnName("PersonID");
                //entity.Property(c => c.StoreId)
                //.HasColumnName("StoreID");
            });

            modelBuilder.Entity<PersonRecord>(entity =>
            {
                entity.ToTable("Person", "dbo");
                entity.HasKey(p => p.BusinessEntityId);
                entity.Property(p => p.BusinessEntityId)
                .HasColumnName("BusinessEntityId");
                entity.Property(p => p.FirstName)
                .HasColumnName("FirstName");
                entity.Property(p => p.LastName)
                .HasColumnName("LastName");
            });

            modelBuilder.Entity<AddressRecord>(entity =>          
            {
                entity.ToTable("Address", "dbo");
                entity.HasKey(a => a.AddressId);
                entity.Property(a => a.AddressId).HasColumnName("AddressID");
                entity.Property(a => a.AddressLine1).HasColumnName("AddressLine1");
                entity.Property(a => a.AddressLine2).HasColumnName("AddressLine2");
                entity.Property(a => a.City).HasColumnName("City");
                entity.Property(a => a.PostalCode).HasColumnName("PostalCode");
            });
        }
    }
}

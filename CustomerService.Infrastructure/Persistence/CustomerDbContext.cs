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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Sales");
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
                entity.ToTable("Person", "Person");
                entity.HasKey(p => p.BusinessEntityId);
                entity.Property(p => p.BusinessEntityId)
                .HasColumnName("BusinessEntityId");
                entity.Property(p => p.FirstName)
                .HasColumnName("FirstName");
                entity.Property(p => p.LastName)
                .HasColumnName("LastName");
            });
        }
    }
}

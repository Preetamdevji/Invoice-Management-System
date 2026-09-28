using InvoiceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceService.Infrastructure.Persistence;

public class InvoiceDbContext : DbContext
{
    public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceDetail> InvoiceDetails => Set<InvoiceDetail>();
    public DbSet<AddressRecord> Addresses => Set<AddressRecord>();
    public DbSet<ShipMethodRecord> ShipMethods => Set<ShipMethodRecord>();
    public DbSet<ProductRecord> Products => Set<ProductRecord>();
    public DbSet<CustomerRecord> Customers => Set<CustomerRecord>();
    public DbSet<PersonRecord> People => Set<PersonRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("SalesOrderHeader", "Sales", tb => tb.UseSqlOutputClause(false));

            entity.HasKey(i => i.SalesOrderId);

            entity.Property(i => i.SalesOrderId).HasColumnName("SalesOrderID");
            entity.Property(i => i.CustomerId).HasColumnName("CustomerID");
            entity.Property(i => i.BillToAddressId).HasColumnName("BillToAddressID");
            entity.Property(i => i.ShipToAddressId).HasColumnName("ShipToAddressID");
            entity.Property(i => i.ShipMethodId).HasColumnName("ShipMethodID");
            entity.Property(i => i.OrderDate).HasColumnName("OrderDate");
            entity.Property(i => i.DueDate).HasColumnName("DueDate");
            entity.Property(i => i.SubTotal).HasColumnName("SubTotal");
            entity.Property(i => i.TaxAmt).HasColumnName("TaxAmt");
            entity.Property(i => i.Freight).HasColumnName("Freight");
            entity.Property(i => i.Comment).HasColumnName("Comment");

            // Computed columns — SQL Server generates these, EF must NEVER try to insert/update them
            entity.Property(i => i.SalesOrderNumber)
                .HasColumnName("SalesOrderNumber")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            entity.Property(i => i.TotalDue)
                .HasColumnName("TotalDue")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            // One-to-many: Invoice -> Details
            entity.HasMany(i => i.Details)
                .WithOne()
                .HasForeignKey(d => d.SalesOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("SalesOrderDetail", "Sales", tb => tb.UseSqlOutputClause(false));

            entity.HasKey(d => d.SalesOrderDetailId);

            entity.Property(d => d.SalesOrderDetailId).HasColumnName("SalesOrderDetailID");
            entity.Property(d => d.SalesOrderId).HasColumnName("SalesOrderID");
            entity.Property(d => d.ProductId).HasColumnName("ProductID");
            entity.Property(d => d.SpecialOfferId).HasColumnName("SpecialOfferID");
            entity.Property(d => d.OrderQty).HasColumnName("OrderQty");
            entity.Property(d => d.UnitPrice).HasColumnName("UnitPrice");
            entity.Property(d => d.UnitPriceDiscount).HasColumnName("UnitPriceDiscount");

            // Computed column
            entity.Property(d => d.LineTotal)
                .HasColumnName("LineTotal")
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
        });

        modelBuilder.Entity<AddressRecord>(entity =>
        {
            entity.ToTable("Address", "Person");

            entity.HasKey(a => a.AddressId);

            entity.Property(a => a.AddressId).HasColumnName("AddressID");
            entity.Property(a => a.AddressLine1).HasColumnName("AddressLine1");
            entity.Property(a => a.City).HasColumnName("City");
            entity.Property(a => a.PostalCode).HasColumnName("PostalCode");
        });

        modelBuilder.Entity<ShipMethodRecord>(entity =>
        {
            entity.ToTable("ShipMethod", "Purchasing");

            entity.HasKey(sm => sm.ShipMethodId);

            entity.Property(sm => sm.ShipMethodId).HasColumnName("ShipMethodID");
            entity.Property(sm => sm.Name).HasColumnName("Name");
        });

        modelBuilder.Entity<ProductRecord>(entity =>
        {
            entity.ToTable("Product", "Production");

            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductId).HasColumnName("ProductID");
            entity.Property(p => p.Name).HasColumnName("Name");
            entity.Property(p => p.ProductNumber).HasColumnName("ProductNumber");
            entity.Property(p => p.Color).HasColumnName("Color");
            entity.Property(p => p.ListPrice).HasColumnName("ListPrice");
        });

        modelBuilder.Entity<CustomerRecord>(entity =>
        {
            entity.ToTable("Customer", "Sales");

            entity.HasKey(c => c.CustomerId);

            entity.Property(c => c.CustomerId).HasColumnName("CustomerID");
            entity.Property(c => c.PersonId).HasColumnName("PersonID");
        });

        modelBuilder.Entity<PersonRecord>(entity =>
        {
            entity.ToTable("Person", "Person");

            entity.HasKey(p => p.BusinessEntityId);

            entity.Property(p => p.BusinessEntityId).HasColumnName("BusinessEntityID");
            entity.Property(p => p.FirstName).HasColumnName("FirstName");
            entity.Property(p => p.LastName).HasColumnName("LastName");
        });
    }
}
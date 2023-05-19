using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DbFirst.ModelsDB;

public partial class DbfirstContext : DbContext
{
    public DbfirstContext()
    {
    }

    public DbfirstContext(DbContextOptions<DbfirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=THINKBOOK;Database=DBFIRST;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OId);

            entity.ToTable("Order");

            entity.Property(e => e.OId)
                .ValueGeneratedNever()
                .HasColumnName("o_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.IId);

            entity.ToTable("orderitem");

            entity.Property(e => e.IId)
                .ValueGeneratedNever()
                .HasColumnName("i_id");
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(7, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Total)
                .HasComputedColumnSql("([amount]*[price])", false)
                .HasColumnType("numeric(27, 6)")
                .HasColumnName("total");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orderitem_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_orderitem_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PId);

            entity.ToTable("product");

            entity.Property(e => e.PId)
                .ValueGeneratedNever()
                .HasColumnName("p_id");
            entity.Property(e => e.PName)
                .HasColumnType("text")
                .HasColumnName("p_name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using dotnet_backend.Models;

namespace dotnet_backend.Database;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SQLite configuration (removed MySQL-specific collation and charset)

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);
            entity.ToTable("categories");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName).HasMaxLength(100).HasColumnName("category_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);
            entity.ToTable("customers");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')").HasColumnName("created_at");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId);
            entity.ToTable("inventory");
            entity.HasIndex(e => e.ProductId, "fk_inventory_products");
            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasDefaultValue(0).HasColumnName("quantity");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("datetime('now')").HasColumnName("updated_at");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.ToTable("orders");
            entity.HasIndex(e => e.CustomerId, "fk_orders_customers");
            entity.HasIndex(e => e.PromoId, "fk_orders_promotions");
            entity.HasIndex(e => e.UserId, "fk_orders_users");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10,2)").HasDefaultValue(0.00m).HasColumnName("discount_amount");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("datetime('now')").HasColumnName("order_date");
            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.Status).HasDefaultValue("pending").HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)").HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Promo).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromoId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);
            entity.ToTable("order_items");
            entity.HasIndex(e => e.OrderId, "fk_order_items_orders");
            entity.HasIndex(e => e.ProductId, "fk_order_items_products");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)").HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)").HasColumnName("subtotal");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);
            entity.ToTable("payments");
            entity.HasIndex(e => e.OrderId, "fk_payments_orders");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount).HasColumnType("decimal(10,2)").HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("datetime('now')").HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod).HasDefaultValue("cash").HasColumnName("payment_method");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId);
            entity.ToTable("permissions");
            entity.HasIndex(e => e.ActionKey, "action_key").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.ActionKey).HasMaxLength(50).HasColumnName("action_key");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.PermissionName).HasMaxLength(100).HasColumnName("permission_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.ToTable("products");
            entity.HasIndex(e => e.Barcode, "barcode").IsUnique();
            entity.HasIndex(e => e.CategoryId, "fk_products_categories");
            entity.HasIndex(e => e.SupplierId, "fk_products_suppliers");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Barcode).HasMaxLength(50).HasColumnName("barcode");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')").HasColumnName("created_at");
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)").HasColumnName("price");
            entity.Property(e => e.ProductName).HasMaxLength(100).HasColumnName("product_name");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Unit).HasMaxLength(20).HasDefaultValue("pcs").HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromoId);
            entity.ToTable("promotions");
            entity.HasIndex(e => e.PromoCode, "promo_code").IsUnique();

            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
            entity.Property(e => e.DiscountType).HasColumnName("discount_type");
            entity.Property(e => e.DiscountValue).HasColumnType("decimal(10,2)").HasColumnName("discount_value");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.MinOrderAmount).HasColumnType("decimal(10,2)").HasDefaultValue(0.00m).HasColumnName("min_order_amount");
            entity.Property(e => e.PromoCode).HasMaxLength(50).HasColumnName("promo_code");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status).HasDefaultValue("active").HasColumnName("status");
            entity.Property(e => e.UsageLimit).HasDefaultValue(0).HasColumnName("usage_limit");
            entity.Property(e => e.UsedCount).HasDefaultValue(0).HasColumnName("used_count");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);
            entity.ToTable("roles");
            entity.HasIndex(e => e.RoleName, "role_name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.RoleName).HasMaxLength(50).HasColumnName("role_name");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.ToTable("role_permissions");
            entity.HasIndex(e => e.PermissionId, "permission_id");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId);
            entity.ToTable("suppliers");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Email).HasMaxLength(100).HasColumnName("email");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.ToTable("users");
            entity.HasIndex(e => e.Role, "role");
            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now')").HasColumnName("created_at");
            entity.Property(e => e.FullName).HasMaxLength(100).HasColumnName("full_name");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Username).HasMaxLength(50).HasColumnName("username");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

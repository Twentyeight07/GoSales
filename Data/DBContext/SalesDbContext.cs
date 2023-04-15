using System;
using System.Collections.Generic;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.DBContext;

public partial class SalesDbContext : DbContext
{
    public SalesDbContext()
    {
    }

    public SalesDbContext(DbContextOptions<SalesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<CorrelativeNumber> CorrelativeNumbers { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleMenu> RoleMenus { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<SaleDocType> SaleDocTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("PK__Business__3C08D48265EA3C47");

            entity.ToTable("Business");

            entity.Property(e => e.BusinessId)
                .ValueGeneratedNever()
                .HasColumnName("businessId");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CurrencySymbol)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("currencySymbol");
            entity.Property(e => e.DocNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("docNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("logoURL");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NombreLogo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreLogo");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.TaxRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("taxRate");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D8A9ABFE76");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Configuration");

            entity.Property(e => e.Property)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("property");
            entity.Property(e => e.Resource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("resource");
            entity.Property(e => e.Value)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("value");
        });

        modelBuilder.Entity<CorrelativeNumber>(entity =>
        {
            entity.HasKey(e => e.CorrelativeNumberId).HasName("PK__Correlat__4B974AD3B2668489");

            entity.ToTable("CorrelativeNumber");

            entity.Property(e => e.CorrelativeNumberId).HasColumnName("correlativeNumberId");
            entity.Property(e => e.DigitsQuantity).HasColumnName("digitsQuantity");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.LastNumber).HasColumnName("lastNumber");
            entity.Property(e => e.Management)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("management");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menu__3B40717450B7E7AD");

            entity.ToTable("Menu");

            entity.Property(e => e.MenuId).HasColumnName("menuId");
            entity.Property(e => e.ActionPage)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("actionPage");
            entity.Property(e => e.Controller)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("controller");
            entity.Property(e => e.Description)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Icon)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.IdParentMenu).HasColumnName("idParentMenu");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");

            entity.HasOne(d => d.IdParentMenuNavigation).WithMany(p => p.InverseIdParentMenuNavigation)
                .HasForeignKey(d => d.IdParentMenu)
                .HasConstraintName("FK__Menu__idParentMe__37A5467C");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D16A14E39BA2");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.BarCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("barCode");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.PicName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("picName");
            entity.Property(e => e.PicUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("picURL");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__categor__49C3F6B7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98462A463A374A");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.Description)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
        });

        modelBuilder.Entity<RoleMenu>(entity =>
        {
            entity.HasKey(e => e.RoleMenuId).HasName("PK__RoleMenu__4E3921ECF39124F0");

            entity.ToTable("RoleMenu");

            entity.Property(e => e.RoleMenuId).HasColumnName("roleMenuId");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.MenuId).HasColumnName("menuId");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Menu).WithMany(p => p.RoleMenus)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__RoleMenu__menuId__3F466844");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleMenus)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK__RoleMenu__roleid__3E52440B");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sale__FAE8F4F509E211BB");

            entity.ToTable("Sale");

            entity.Property(e => e.SaleId).HasColumnName("saleId");
            entity.Property(e => e.ClientDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clientDoc");
            entity.Property(e => e.ClientName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("clientName");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
            entity.Property(e => e.SaleDocTypeId).HasColumnName("saleDocTypeId");
            entity.Property(e => e.SaleNumber)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("saleNumber");
            entity.Property(e => e.SubTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subTotal");
            entity.Property(e => e.TaxTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("taxTotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.SaleDocType).WithMany(p => p.Sales)
                .HasForeignKey(d => d.SaleDocTypeId)
                .HasConstraintName("FK__Sale__saleDocTyp__52593CB8");

            entity.HasOne(d => d.User).WithMany(p => p.Sales)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sale__userId__534D60F1");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PK__SaleDeta__8CFF41D63A965305");

            entity.ToTable("SaleDetail");

            entity.Property(e => e.SaleDetailId).HasColumnName("saleDetailId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductBrand)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("productBrand");
            entity.Property(e => e.ProductCategory)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("productCategory");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("productDescription");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SaleId).HasColumnName("saleId");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK__SaleDetai__saleI__571DF1D5");
        });

        modelBuilder.Entity<SaleDocType>(entity =>
        {
            entity.HasKey(e => e.SaleDocTypeId).HasName("PK__SaleDocT__D8B753B64EF0695B");

            entity.ToTable("SaleDocType");

            entity.Property(e => e.SaleDocTypeId).HasColumnName("saleDocTypeId");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CFF4442B3D9");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PicName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("picName");
            entity.Property(e => e.PicUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("picURL");
            entity.Property(e => e.RegistryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("registryDate");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK__Users__idRole__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

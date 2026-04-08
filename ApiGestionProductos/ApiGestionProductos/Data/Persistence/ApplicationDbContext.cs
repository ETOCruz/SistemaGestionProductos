using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<OrderEntity> Order { get; set; }
        public DbSet<OrderDetailEntity> OrderDetail { get; set; }
        public DbSet<WarehouseEntity> Warehouse { get; set; }
        public DbSet<InventoryEntity> Inventory { get; set; }
        public DbSet<OrderStatusHistoryEntity> OrderStatusHistory { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional del modelo para ProductEntity
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Barcode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.Barcode)
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Ignore(e => e.ProductFullDescription);

                entity.Property<DateTime>("CreateAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdatedAt")
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.SubCategory)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.SubCategoryId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración adicional del modelo para WarehouseEntity
            modelBuilder.Entity<WarehouseEntity>(entity =>
            {
                entity.ToTable("Warehouses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Semilla de datos para las 2 bodegas requeridas
            var warehouseAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var warehouseBId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<WarehouseEntity>().HasData(
                new { Id = warehouseAId, Name = "Bodega Central" },
                new { Id = warehouseBId, Name = "Bodega Secundaria" }
            );

            // Configuración adicional del modelo para InventoryEntity
            modelBuilder.Entity<InventoryEntity>(entity =>
            {
                entity.ToTable("Inventories");
                entity.HasKey(e => new { e.ProductId, e.WarehouseId });

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId);

                entity.HasOne(e => e.Warehouse)
                    .WithMany()
                    .HasForeignKey(e => e.WarehouseId);

                entity.Property(e => e.StockQuantity).IsRequired();
            });

            // Configuración adicional del modelo para OrderEntity
            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Folio)
                .HasMaxLength(20);
                
                entity.HasIndex(e => e.Folio)
                .IsUnique();
                
                entity.Property(e => e.Status)
                .IsRequired();

                entity.Property(e => e.SellerId)
                .IsRequired();
            });

            // Configuración adicional del modelo para OrderStatusHistoryEntity
            modelBuilder.Entity<OrderStatusHistoryEntity>(entity =>
            {
                entity.ToTable("OrderStatusHistory");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.ChangedAt).IsRequired();
                entity.Property(e => e.Observation).HasMaxLength(250);
            });

            // Configuración adicional del modelo para OrderDetailEntity
            modelBuilder.Entity<OrderDetailEntity>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.Details)
                    .HasForeignKey(e => e.OrderId);

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId);
            });

            // Configuración adicional del modelo para CategoryEntity
            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Configuración adicional del modelo para SubCategoryEntity
            modelBuilder.Entity<SubCategoryEntity>(entity =>
            {
                entity.ToTable("SubCategories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(e => e.CategoryId);
            });

            // Semilla de datos para Categorías y Subcategorías
            int techId = 1;
            int homeId = 2;

            modelBuilder.Entity<CategoryEntity>().HasData(
                new { Id = techId, Name = "Tecnología" },
                new { Id = homeId, Name = "Hogar" }
            );

            modelBuilder.Entity<SubCategoryEntity>().HasData(
                new { Id = 1, Name = "Laptops", CategoryId = techId },
                new { Id = 2, Name = "Celulares", CategoryId = techId },
                new { Id = 3, Name = "Muebles", CategoryId = homeId },
                new { Id = 4, Name = "Cocina", CategoryId = homeId }
            );

            // Semilla de Productos
            var p1Id = Guid.Parse("46f3f79d-0488-4f61-abcd-517cce578cd4");
            var p2Id = Guid.Parse("d0151aab-c32e-4b50-b629-e380f50342c8");
            var p3Id = Guid.Parse("20624587-9688-4842-801e-5a98219c3fb6");
            var p4Id = Guid.Parse("a1779efe-cdb4-4e67-9dd0-5da791841132");
            var p5Id = Guid.Parse("39a0bd15-b635-422a-b1bf-54dc66cb1581");
            var p6Id = Guid.Parse("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec");

            modelBuilder.Entity<ProductEntity>().HasData(
                new { Id = p1Id, Barcode = "123456789", Name = "Asus Vivobook", ProductDescription = "The ASUS Vivobook is a versatile", Price = 25.50m, SubCategoryId = 2 },
                new { Id = p2Id, Barcode = "163062", Name = "Escritorio de baal con 3 cajones color nogal", ProductDescription = "Modelo Baal walnut b", Price = 25.50m, SubCategoryId = 3 },
                new { Id = p3Id, Barcode = "198866", Name = "Tarja eb de 1 tinas", ProductDescription = "Modelo Eb onen33229-1012hf", Price = 25.50m, SubCategoryId = 4 },
                new { Id = p4Id, Barcode = "198865", Name = "Tarja eb de 2 tinas", ProductDescription = "Modelo Eb onen33229-1012hd", Price = 25.50m, SubCategoryId = 4 },
                new { Id = p5Id, Barcode = "198867", Name = "Tarja eb de 3 tinas", ProductDescription = "Modelo Eb onen33229-1012ht", Price = 25.50m, SubCategoryId = 4 },
                new { Id = p6Id, Barcode = "1234563", Name = "Xiaomi POCO F7 Ultra", ProductDescription = "El contenido del paquete puede variar según la región.", Price = 25.50m, SubCategoryId = 2 }
            );

            // Semilla de Inventarios (100 unidades en la Bodega 1 y 50 en la Bodega 2)
            modelBuilder.Entity<InventoryEntity>().HasData(
                new { ProductId = p1Id, WarehouseId = warehouseAId, StockQuantity = 100 },
                new { ProductId = p2Id, WarehouseId = warehouseAId, StockQuantity = 100 },
                new { ProductId = p3Id, WarehouseId = warehouseAId, StockQuantity = 100 },
                new { ProductId = p4Id, WarehouseId = warehouseAId, StockQuantity = 100 },
                new { ProductId = p5Id, WarehouseId = warehouseAId, StockQuantity = 100 },
                new { ProductId = p6Id, WarehouseId = warehouseAId, StockQuantity = 100 },

                new { ProductId = p1Id, WarehouseId = warehouseBId, StockQuantity = 50 },
                new { ProductId = p2Id, WarehouseId = warehouseBId, StockQuantity = 50 },
                new { ProductId = p3Id, WarehouseId = warehouseBId, StockQuantity = 50 },
                new { ProductId = p4Id, WarehouseId = warehouseBId, StockQuantity = 50 },
                new { ProductId = p5Id, WarehouseId = warehouseBId, StockQuantity = 50 },
                new { ProductId = p6Id, WarehouseId = warehouseBId, StockQuantity = 50 }
            );

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // extras
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Metadata.FindProperty("UpdatedAt") != null)
                { // Se aplica esta regla solo si existe esta columnas en la tabla
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }

    }
}

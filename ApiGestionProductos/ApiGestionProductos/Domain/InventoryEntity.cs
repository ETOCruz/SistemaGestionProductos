using System;

namespace Domain
{
    public class InventoryEntity
    {
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int StockQuantity { get; set; }

        public ProductEntity Product { get; set; } = null!;
        public WarehouseEntity Warehouse { get; set; } = null!;

        public void AddStock(int quantity)
        {
            if (quantity < 0) throw new ArgumentException("La cantidad no puede ser negativa.");
            StockQuantity += quantity;
        }

        public void RemoveStock(int quantity)
        {
            if (quantity < 0) throw new ArgumentException("La cantidad no puede ser negativa.");
            if (StockQuantity < quantity)
                throw new InvalidOperationException("Stock insuficiente en la bodega seleccionada.");

            StockQuantity -= quantity;
        }

        public InventoryEntity(Guid productId, Guid warehouseId, int initialStock)
        {
            ProductId = productId;
            WarehouseId = warehouseId;
            StockQuantity = initialStock;
        }

        // Para EF Core
        private InventoryEntity() { }
    }
}

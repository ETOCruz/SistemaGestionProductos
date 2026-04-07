using System;

namespace Domain
{
    public class OrderDetailEntity
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityOrdered { get; private set; }
        public int QuantityScanned { get; private set; }

        public OrderEntity Order { get; set; } = null!;
        public ProductEntity Product { get; set; } = null!;

        public OrderDetailEntity(Guid productId, int quantityOrdered)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            QuantityOrdered = quantityOrdered;
            QuantityScanned = 0;
        }

        public void Scan(int quantity)
        {
            if (QuantityScanned + quantity > QuantityOrdered)
                throw new InvalidOperationException($"No se puede surtir más de lo solicitado ({QuantityOrdered}).");

            QuantityScanned += quantity;
        }

        // Para EF Core
        private OrderDetailEntity() { }
    }
}

using Domain.Enums;
using System;

namespace Domain
{
    public class OrderStatusHistoryEntity
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public OrderStatus Status { get; private set; }
        public Guid? UserId { get; private set; }
        public DateTime ChangedAt { get; private set; }
        public string? Observation { get; private set; }

        public OrderStatusHistoryEntity(Guid orderId, OrderStatus status, Guid? userId = null, string? observation = null)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Status = status;
            UserId = userId;
            ChangedAt = DateTime.UtcNow;
            Observation = observation;
        }

        // Constructor para EF
        #pragma warning disable CS8618
        private OrderStatusHistoryEntity() { }
        #pragma warning restore CS8618
    }
}

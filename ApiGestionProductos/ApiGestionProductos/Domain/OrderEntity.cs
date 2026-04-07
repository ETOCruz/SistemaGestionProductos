using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; private set; }
        public string? Folio { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public ICollection<OrderDetailEntity> Details { get; set; } = new List<OrderDetailEntity>();

        public OrderEntity(Guid sellerId)
        {
            Id = Guid.NewGuid();
            SellerId = sellerId;
            Status = OrderStatus.Cotizacion;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Authorize(string folio)
        {
            if (Status != OrderStatus.Cotizacion)
                throw new InvalidOperationException("Solo se pueden autorizar órdenes en estado de Cotización.");

            Folio = folio;
            Status = OrderStatus.Autorizacion;
            UpdatedAt = DateTime.UtcNow;
        }

        public void StartPicking()
        {
            if (Status != OrderStatus.Autorizacion && Status != OrderStatus.Pausa)
                throw new InvalidOperationException("La orden debe estar autorizada o pausada para iniciar el surtido.");

            Status = OrderStatus.EnSurtido;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Pause()
        {
            Status = OrderStatus.Pausa;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            Status = OrderStatus.SurtidoFinalizado;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

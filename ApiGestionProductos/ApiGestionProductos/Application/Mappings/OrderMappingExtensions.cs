using Application.DTOs.Orders;
using Domain;
using System.Linq;

namespace Application.Mappings
{
    public static class OrderMappingExtensions
    {
        public static OrderResponseDto ToDto(this OrderEntity order)
        {
            if (order == null) return null!;

            return new OrderResponseDto
            {
                Id = order.Id,
                Folio = order.Folio,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                Details = order.Details?.Select(d => d.ToDto()).ToList() ?? new System.Collections.Generic.List<OrderDetailResponseDto>()
            };
        }

        public static OrderDetailResponseDto ToDto(this OrderDetailEntity detail)
        {
            if (detail == null) return null!;

            return new OrderDetailResponseDto
            {
                ProductId = detail.ProductId,
                ProductName = detail.Product?.Name ?? "N/A",
                QuantityOrdered = detail.QuantityOrdered,
                QuantityScanned = detail.QuantityScanned
            };
        }
    }
}

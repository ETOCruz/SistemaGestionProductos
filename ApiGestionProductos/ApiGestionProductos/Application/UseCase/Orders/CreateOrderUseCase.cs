using Application.DTOs.Orders;
using Application.Mappings;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Orders
{
    public class CreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusHistoryRepository _historyRepository;

        public CreateOrderUseCase(IOrderRepository orderRepository, IOrderStatusHistoryRepository historyRepository)
        {
            _orderRepository = orderRepository;
            _historyRepository = historyRepository;
        }

        public async Task<OrderResponseDto> ExecuteAsync(CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("La orden debe tener al menos un producto.");

            if (dto.SellerId == Guid.Empty)
                throw new ArgumentException("El ID del vendedor es obligatorio.");

            var order = new OrderEntity(dto.SellerId);

            foreach (var item in dto.Items)
            {
                var detail = new OrderDetailEntity(item.ProductId, item.Quantity);
                order.Details.Add(detail);
            }

            await _orderRepository.AddAsync(order);
            
            // Registro inicial en el historial
            var history = new OrderStatusHistoryEntity(order.Id, order.Status, dto.SellerId, "Creación de la cotización por el vendedor.");
            await _historyRepository.AddAsync(history);

            await _orderRepository.SaveChangesAsync();
            await _historyRepository.SaveChangesAsync();

            return order.ToDto();
        }
    }
}

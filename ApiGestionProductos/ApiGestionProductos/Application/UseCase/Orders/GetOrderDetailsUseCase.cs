using Application.Mappings;
using Domain.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Orders;
using System.Collections.Generic;

namespace Application.UseCase.Orders
{
    public class GetOrderDetailsUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderDetailsUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDto> ExecuteAsync(Guid orderId)
        {
            var order = await _orderRepository.GetWithDetailsAsync(orderId);
            if (order == null) throw new InvalidOperationException("Orden no encontrada.");

            var dto = order.ToDto();
            dto.StatusId = (int)order.Status;
            return dto;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var (orders, _) = await _orderRepository.GetPagedAsync(1, 100); // Legacy or fallback
            return orders.Select(o => {
                var dto = o.ToDto();
                dto.StatusId = (int)o.Status;
                return dto;
            });
        }
    }
}

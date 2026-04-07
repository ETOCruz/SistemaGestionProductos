using Application.DTOs;
using Application.DTOs.Orders;
using Application.Mappings;
using Domain.Abstractions;
using Domain.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Orders
{
    public class GetAllOrdersUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PagedResponseDto<OrderResponseDto>> ExecuteAsync(OrderStatus? status, int pageNumber, int pageSize)
        {
            if (pageSize <= 0) pageSize = 10;
            if (pageNumber <= 0) pageNumber = 1;

            var (items, totalCount) = await _orderRepository.GetPagedByStatusAsync(status, pageNumber, pageSize);
            
            var dtos = items.Select(o => o.ToDto());
            
            // Populate StatusId specifically if not already in ToDto
            foreach (var dto in dtos)
            {
                var originalOrder = items.First(o => o.Id == dto.Id);
                dto.StatusId = (int)originalOrder.Status;
            }

            return new PagedResponseDto<OrderResponseDto>(dtos, totalCount, pageNumber, pageSize);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IOrderStatusHistoryRepository : IRepository<OrderStatusHistoryEntity, Guid>
    {
        Task<IEnumerable<OrderStatusHistoryEntity>> GetByOrderIdAsync(Guid orderId);
    }
}

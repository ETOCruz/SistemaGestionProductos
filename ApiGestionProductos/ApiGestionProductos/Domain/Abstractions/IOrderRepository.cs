using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Abstractions
{
    public interface IOrderRepository : IRepository<OrderEntity, Guid>
    {
        Task<OrderEntity?> GetWithDetailsAsync(Guid id);
        Task<OrderEntity?> GetByFolioAsync(string folio);
        Task<string> GetNextFolioAsync();
        Task<(IEnumerable<OrderEntity> Items, int TotalCount)> GetPagedByStatusAsync(OrderStatus? status, int pageNumber, int pageSize);
    }
}

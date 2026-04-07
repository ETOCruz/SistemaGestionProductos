using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryEntity>> GetByProductIdAsync(Guid productId);
        Task<InventoryEntity?> GetProductStockInWarehouseAsync(Guid productId, Guid warehouseId);
        Task UpdateAsync(InventoryEntity inventory);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<InventoryEntity>> GetAllByProductIdAsync(Guid productId);
    }
}

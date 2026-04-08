using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryEntity>> GetByProductIdAsync(Guid productId)
        {
            return await _context.Inventory
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        public async Task<InventoryEntity?> GetProductStockInWarehouseAsync(Guid productId, Guid warehouseId)
        {
            return await _context.Inventory
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);
        }

        public Task UpdateAsync(InventoryEntity inventory)
        {
            _context.Inventory.Update(inventory);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryEntity>> GetAllByProductIdAsync(Guid productId)
        {
            return await _context.Inventory
                .Where(i => i.ProductId == productId)
                .OrderByDescending(i => i.StockQuantity)
                .ToListAsync();
        }

        public async Task AddAsync(InventoryEntity inventory)
        {
            await _context.Inventory.AddAsync(inventory);
        }
    }
}

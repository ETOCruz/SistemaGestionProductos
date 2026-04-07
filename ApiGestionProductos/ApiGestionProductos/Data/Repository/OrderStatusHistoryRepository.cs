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
    public class OrderStatusHistoryRepository : IOrderStatusHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderStatusHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderStatusHistoryEntity entity)
        {
            await _context.OrderStatusHistory.AddAsync(entity);
        }

        public Task DeleteAsync(OrderStatusHistoryEntity entity)
        {
            _context.OrderStatusHistory.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderStatusHistoryEntity>> GetAllAsync()
        {
            return await _context.OrderStatusHistory.AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<OrderStatusHistoryEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.OrderStatusHistory.AsNoTracking();
            int totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(h => h.ChangedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<OrderStatusHistoryEntity?> GetByIdAsync(Guid id)
        {
            return await _context.OrderStatusHistory.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatusHistoryEntity>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderStatusHistory
                .Where(h => h.OrderId == orderId)
                .OrderBy(h => h.ChangedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(OrderStatusHistoryEntity entity)
        {
            _context.OrderStatusHistory.Update(entity);
            return Task.CompletedTask;
        }
    }
}

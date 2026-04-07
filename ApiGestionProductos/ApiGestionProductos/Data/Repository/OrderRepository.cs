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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderEntity entity)
        {
            await _context.Order.AddAsync(entity);
        }

        public Task DeleteAsync(OrderEntity entity)
        {
            _context.Order.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync()
        {
            return await _context.Order
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<OrderEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Order.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<OrderEntity?> GetByFolioAsync(string folio)
        {
            return await _context.Order
                .FirstOrDefaultAsync(o => o.Folio == folio);
        }

        public async Task<string> GetNextFolioAsync()
        {
            var count = await _context.Order.CountAsync(o => o.Folio != null);
            return $"ORD-{(count + 1):D3}";
        }

        public async Task<OrderEntity?> GetWithDetailsAsync(Guid id)
        {
            return await _context.Order
                .Include(o => o.Details)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<(IEnumerable<OrderEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Order
                .Include(o => o.Details)
                .ThenInclude(d => d.Product)
                .AsNoTracking();

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<OrderEntity> Items, int TotalCount)> GetPagedByStatusAsync(Domain.Enums.OrderStatus? status, int pageNumber, int pageSize)
        {
            var query = _context.Order
                .Include(o => o.Details)
                .ThenInclude(d => d.Product)
                .AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(OrderEntity entity)
        {
            _context.Order.Update(entity);
            return Task.CompletedTask;
        }
    }
}

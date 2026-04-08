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
    public class WarehouseRepository : IRepository<WarehouseEntity, Guid>
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WarehouseEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.Warehouse.AddAsync(entity);
        }

        public Task DeleteAsync(WarehouseEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Warehouse.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<WarehouseEntity>> GetAllAsync()
        {
            return await _context.Warehouse
                .AsNoTracking()
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<WarehouseEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Warehouse.AsNoTracking();
            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(w => w.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<WarehouseEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Warehouse.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(WarehouseEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.Warehouse.Update(entity);
            return Task.CompletedTask;
        }
    }
}

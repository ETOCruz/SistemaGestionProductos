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
    public class CategoryRepository : IRepository<CategoryEntity, int>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CategoryEntity entity)
        {
            await _context.Categories.AddAsync(entity);
        }

        public Task DeleteAsync(CategoryEntity entity)
        {
            _context.Categories.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<CategoryEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Categories.AsNoTracking();
            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<CategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(CategoryEntity entity)
        {
            _context.Categories.Update(entity);
            return Task.CompletedTask;
        }
    }

    public class SubCategoryRepository : IRepository<SubCategoryEntity, int>
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SubCategoryEntity entity)
        {
            await _context.SubCategories.AddAsync(entity);
        }

        public Task DeleteAsync(SubCategoryEntity entity)
        {
            _context.SubCategories.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<SubCategoryEntity>> GetAllAsync()
        {
            return await _context.SubCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<SubCategoryEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.SubCategories.AsNoTracking();
            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<SubCategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(SubCategoryEntity entity)
        {
            _context.SubCategories.Update(entity);
            return Task.CompletedTask;
        }
    }
}

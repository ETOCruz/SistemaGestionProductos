using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class ProductRepository : IProductRepository,
        ICodeRepository<ProductEntity>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region IRepository implementation
        public async Task AddAsync(ProductEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Product.AddAsync(entity);
        }

        public Task DeleteAsync(ProductEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Product.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .AsNoTracking()
                .OrderBy(p => p.Barcode)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .AsNoTracking();

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<ProductEntity> Items, int TotalCount)> SearchByNameAsync(string name, int pageNumber, int pageSize)
        {
            var query = _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .Where(p => p.Name.Contains(name))
                .AsNoTracking();

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetBySubCategoryAsync(int subCategoryId, int pageNumber, int pageSize)
        {
            var query = _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .Where(p => p.SubCategoryId == subCategoryId)
                .AsNoTracking();

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetByCategoryAsync(int categoryId, int pageNumber, int pageSize)
        {
            var query = _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .Where(p => p.SubCategory != null && p.SubCategory.CategoryId == categoryId)
                .AsNoTracking();

            int totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<ProductEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(ProductEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Product.Update(entity);

            return Task.CompletedTask;
        }
        #endregion

        #region ICodeRepository implementation
        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) { 
                throw new ArgumentException("El barcode no puede estar vacío", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();

            return await _context.Product.AnyAsync(p => p.Barcode == normalizedCode);
        }

        public async Task<ProductEntity?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) { 
                throw new ArgumentException("El barcode no puede estar vacío", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();

            return await _context.Product
                .Include(p => p.SubCategory)
                .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(p => p.Barcode == normalizedCode);
        }
        #endregion
    }
}

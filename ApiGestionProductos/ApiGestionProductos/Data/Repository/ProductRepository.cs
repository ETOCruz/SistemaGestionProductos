using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class ProductRepository : IRepository<ProductEntity, Guid>,
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
                .AsNoTracking() // aqui se ahorra memoria ya que esto es información de solo lectura
                .OrderBy(p => p.Barcode)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProductEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Product
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
        public async Task<bool> ExistsWithCodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) { 
                throw new ArgumentException("El código no puede estar vacío", nameof(barcode));
            }

            var normalizedCode = barcode.ToUpperInvariant();

            return await _context.Product.AnyAsync(p => p.Barcode == normalizedCode);
        }

        public async Task<ProductEntity?> GetByCodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) { 
                throw new ArgumentException("El barcode no puede estar vacío", nameof(barcode));
            }

            var normalizedCode = barcode.ToUpperInvariant();

            return await _context.Product.FirstOrDefaultAsync(p => p.Barcode == normalizedCode);
        }
        #endregion
    }
}

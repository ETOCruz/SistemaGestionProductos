using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IProductRepository : IRepository<ProductEntity, Guid>
    {
        Task<(IEnumerable<ProductEntity> Items, int TotalCount)> SearchByNameAsync(string name, int pageNumber, int pageSize);
        Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetBySubCategoryAsync(int subCategoryId, int pageNumber, int pageSize);
        Task<(IEnumerable<ProductEntity> Items, int TotalCount)> GetByCategoryAsync(int categoryId, int pageNumber, int pageSize);
        Task<bool> ExistsWithCodeAsync(string code);
        Task<ProductEntity?> GetByCodeAsync(string code);
    }
}

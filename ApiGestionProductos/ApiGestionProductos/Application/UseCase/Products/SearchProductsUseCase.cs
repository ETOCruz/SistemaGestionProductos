using Application.DTOs;
using Application.DTOs.Products;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Products
{
    public class SearchProductsUseCase
    {
        private readonly IProductRepository _productRepository;

        public SearchProductsUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResponseDto<ProductResponseDto>> ExecuteAsync(
            string? name, 
            int? categoryId, 
            int? subCategoryId, 
            int pageNumber, 
            int pageSize)
        {
            // Backend limit of 50
            if (pageSize > 50) pageSize = 50;
            if (pageSize <= 0) pageSize = 10;
            if (pageNumber <= 0) pageNumber = 1;

            IEnumerable<ProductEntity> items;
            int totalCount;

            if (!string.IsNullOrWhiteSpace(name))
            {
                (items, totalCount) = await _productRepository.SearchByNameAsync(name, pageNumber, pageSize);
            }
            else if (subCategoryId.HasValue)
            {
                (items, totalCount) = await _productRepository.GetBySubCategoryAsync(subCategoryId.Value, pageNumber, pageSize);
            }
            else if (categoryId.HasValue)
            {
                (items, totalCount) = await _productRepository.GetByCategoryAsync(categoryId.Value, pageNumber, pageSize);
            }
            else
            {
                (items, totalCount) = await _productRepository.GetPagedAsync(pageNumber, pageSize);
            }

            var dtos = items.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Barcode = p.Barcode,
                Name = p.Name,
                Description = p.ProductDescription,
                Price = p.Price,
                SubCategoryId = p.SubCategoryId,
                SubCategoryName = p.SubCategory?.Name ?? "N/A",
                CategoryId = p.SubCategory?.Category?.Id,
                CategoryName = p.SubCategory?.Category?.Name ?? "N/A"
            });

            return new PagedResponseDto<ProductResponseDto>(dtos, totalCount, pageNumber, pageSize);
        }
    }
}

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
    public class GetAllProductsUseCase
    {
        private readonly IProductRepository _repository;

        public GetAllProductsUseCase(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponseDto<ProductResponseDto>> ExecuteAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize);
            
            var dtos = items.Select(ProductResponseDto.FromEntity);

            return new PagedResponseDto<ProductResponseDto>(dtos, totalCount, pageNumber, pageSize);
        }
    }
}

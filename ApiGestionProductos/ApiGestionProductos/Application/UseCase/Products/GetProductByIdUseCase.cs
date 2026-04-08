using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

using Application.DTOs.Products;

namespace Application.UseCase.Products
{
    public class GetProductByIdUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public GetProductByIdUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ProductResponseDto> ExecuteAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró una producto con el Id: {id}");
            }

            return ProductResponseDto.FromEntity(product);
        }
    }
}

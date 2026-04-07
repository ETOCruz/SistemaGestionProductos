using Application.DTOs.Producs;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCase.Products
{
    public class UpdateProductUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public UpdateProductUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<ProductEntity> ExecuteAsync(UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(dto.Id);

            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró una producto con el ID: {dto.Id}");
            }

            product.UpdateProduct(dto.Barcode, dto.Price, dto.Name, dto.ProductDescription, dto.SubCategoryId);

            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return await _repository.GetByIdAsync(product.Id) ?? product;
        }

    }
}

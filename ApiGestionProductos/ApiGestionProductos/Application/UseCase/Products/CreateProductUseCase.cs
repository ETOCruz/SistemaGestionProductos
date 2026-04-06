using Application.DTOs.Producs;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCase.Products
{
    public class CreateProductUseCase
    {
        private readonly ICodeRepository<ProductEntity> _codeRepository;
        private readonly IRepository<ProductEntity, Guid> _repository;

        public CreateProductUseCase(ICodeRepository<ProductEntity> codeRepository, 
            IRepository<ProductEntity, Guid> repository)
        {
            _codeRepository = codeRepository;
            _repository = repository;
        }

        public async Task<ProductEntity> ExecuteAsync(CreateProductDto dto)
        {
            if (await _codeRepository.ExistsWithCodeAsync(dto.Barcode))
            {
                throw new InvalidOperationException("El barcode ya existe en el sistema.");
            }
            var product = new ProductEntity(dto.Barcode, dto.Price, dto.Name, dto.ProductQuantity, dto.ProductDescription);

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }

    }
}

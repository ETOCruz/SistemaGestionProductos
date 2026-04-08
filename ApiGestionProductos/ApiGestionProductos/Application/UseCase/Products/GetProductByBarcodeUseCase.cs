using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

using Application.DTOs.Products;

namespace Application.UseCase.Products
{
    public class GetProductByBarcodeUseCase
    {
        private readonly ICodeRepository<ProductEntity> _codeRepository;

        public GetProductByBarcodeUseCase(ICodeRepository<ProductEntity> codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public async Task<ProductResponseDto> ExecuteAsync(string barcode)
        {
            var product = await _codeRepository.GetByCodeAsync(barcode);

            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró una producto con el código: {barcode}");
            }

            return ProductResponseDto.FromEntity(product);
        }
    }
}

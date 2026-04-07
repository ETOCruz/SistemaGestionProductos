using Application.DTOs.Orders;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Orders
{
    public class ValidateStockUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;

        public ValidateStockUseCase(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ValidateStockResponseDto> ExecuteAsync(ValidateStockRequestDto request)
        {
            var response = new ValidateStockResponseDto { IsAvailable = true };
            
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                var inventoryItems = await _inventoryRepository.GetAllByProductIdAsync(item.ProductId);
                int totalStock = inventoryItems.Sum(i => i.StockQuantity);

                bool isShortage = totalStock < item.Quantity;
                if (isShortage) response.IsAvailable = false;

                response.Details.Add(new ValidateStockDetailDto
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    RequestedQuantity = item.Quantity,
                    AvailableStock = totalStock,
                    IsShortage = isShortage
                });
            }

            return response;
        }
    }
}

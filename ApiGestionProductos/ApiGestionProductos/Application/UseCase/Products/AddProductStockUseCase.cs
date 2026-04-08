using Application.DTOs.Producs;
using Domain;
using Domain.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Products
{
    public class AddProductStockUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRepository<WarehouseEntity, Guid> _warehouseRepository;

        public AddProductStockUseCase(
            IRepository<ProductEntity, Guid> productRepository,
            IInventoryRepository inventoryRepository,
            IRepository<WarehouseEntity, Guid> warehouseRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _warehouseRepository = warehouseRepository;
        }

        public async Task<bool> ExecuteAsync(Guid productId, AddStockDto dto)
        {
            if (dto.QuantityToAdd <= 0)
            {
                throw new ArgumentException("La cantidad a añadir debe ser mayor a 0.");
            }

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró un producto con el ID: {productId}");
            }

            var inventories = await _inventoryRepository.GetByProductIdAsync(productId);
            var inventory = inventories.FirstOrDefault();

            if (inventory != null)
            {
                inventory.AddStock(dto.QuantityToAdd);
                await _inventoryRepository.UpdateAsync(inventory);
            }
            else
            {
                var warehouses = await _warehouseRepository.GetAllAsync();
                var warehouse = warehouses.FirstOrDefault();

                if (warehouse == null)
                {
                    throw new InvalidOperationException("No existe ninguna bodega registrada en el sistema para asignar inventario.");
                }

                inventory = new InventoryEntity(productId, warehouse.Id, dto.QuantityToAdd);
                await _inventoryRepository.AddAsync(inventory);
            }

            await _inventoryRepository.SaveChangesAsync();
            return true;
        }
    }
}

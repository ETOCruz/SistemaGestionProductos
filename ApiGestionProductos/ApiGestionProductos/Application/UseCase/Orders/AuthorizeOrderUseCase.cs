using Application.DTOs.Orders;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Orders
{
    public class AuthorizeOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IOrderStatusHistoryRepository _historyRepository;

        public AuthorizeOrderUseCase(
            IOrderRepository orderRepository, 
            IInventoryRepository inventoryRepository,
            IOrderStatusHistoryRepository historyRepository)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _historyRepository = historyRepository;
        }

        public async Task<AuthorizationResponseDto> ExecuteAsync(Guid orderId, Guid userId)
        {
            var order = await _orderRepository.GetWithDetailsAsync(orderId);
            if (order == null) throw new InvalidOperationException("Orden no encontrada.");

            if (order.Status != Domain.Enums.OrderStatus.Cotizacion)
                throw new InvalidOperationException("La orden ya ha sido procesada.");

            var missingProducts = new List<MissingProductDto>();

            // 1. Validar disponibilidad TOTAL
            foreach (var detail in order.Details)
            {
                var inventories = await _inventoryRepository.GetAllByProductIdAsync(detail.ProductId);
                var totalAvailable = inventories.Sum(i => i.StockQuantity);

                if (totalAvailable < detail.QuantityOrdered)
                {
                    missingProducts.Add(new MissingProductDto
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product?.Name ?? "Producto desconocido",
                        RequestedQuantity = detail.QuantityOrdered,
                        AvailableQuantity = totalAvailable
                    });
                }
            }

            if (missingProducts.Any())
            {
                // Simulación de notificación a proveedores
                foreach (var missing in missingProducts)
                {
                    Console.WriteLine($"[NOTIFICACIÓN PROVEEDORES] Stock insuficiente para {missing.ProductName} (ID: {missing.ProductId}). Falta reabastecer {missing.RequestedQuantity - missing.AvailableQuantity} unidades. Solicitud enviada automáticamente.");
                }

                // Registrar intento fallido en el historial (opcional, para trazabilidad)
                var failHistory = new OrderStatusHistoryEntity(
                    order.Id, 
                    order.Status, 
                    userId, 
                    $"Intento de autorización fallido por falta de inventario en {missingProducts.Count} productos.");
                await _historyRepository.AddAsync(failHistory);
                await _historyRepository.SaveChangesAsync();

                return new AuthorizationResponseDto
                {
                    Success = false,
                    Message = "No hay suficiente inventario para autorizar la orden.",
                    MissingProducts = missingProducts
                };
            }

            // 2. Descontar Inventario Automáticamente (priorizando bodega con más stock)
            foreach (var detail in order.Details)
            {
                var remainingToDiscount = detail.QuantityOrdered;
                var inventories = await _inventoryRepository.GetAllByProductIdAsync(detail.ProductId);

                foreach (var inv in inventories)
                {
                    if (remainingToDiscount <= 0) break;

                    int discount = Math.Min(inv.StockQuantity, remainingToDiscount);
                    inv.RemoveStock(discount);
                    remainingToDiscount -= discount;
                    await _inventoryRepository.UpdateAsync(inv);
                }
            }

            // 3. Autorizar Orden y Generar Folio
            var folio = await _orderRepository.GetNextFolioAsync();
            order.Authorize(folio);

            await _orderRepository.UpdateAsync(order);
            
            // Registro de éxito en el historial
            var history = new OrderStatusHistoryEntity(order.Id, order.Status, userId, $"Orden autorizada por el Jefe de Bodega. Folio generado: {folio}");
            await _historyRepository.AddAsync(history);

            await _orderRepository.SaveChangesAsync();
            await _inventoryRepository.SaveChangesAsync();
            await _historyRepository.SaveChangesAsync();

            return new AuthorizationResponseDto
            {
                Success = true,
                Message = "Orden autorizada exitosamente.",
                Folio = folio
            };
        }
    }
}

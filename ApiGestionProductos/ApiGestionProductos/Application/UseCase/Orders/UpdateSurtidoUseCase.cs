using Application.DTOs.Orders;
using Application.Mappings;
using Domain;
using Domain.Abstractions;
using Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Orders
{
    public class UpdateSurtidoUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICodeRepository<ProductEntity> _productRepository;
        private readonly IOrderStatusHistoryRepository _historyRepository;

        public UpdateSurtidoUseCase(
            IOrderRepository orderRepository, 
            ICodeRepository<ProductEntity> productRepository,
            IOrderStatusHistoryRepository historyRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _historyRepository = historyRepository;
        }

        public async Task<OrderResponseDto> ExecuteAsync(Guid orderId, ScanProductDto dto)
        {
            var order = await _orderRepository.GetWithDetailsAsync(orderId);
            if (order == null) throw new InvalidOperationException("Orden no encontrada.");

            if (order.Status != OrderStatus.Autorizacion && order.Status != OrderStatus.EnSurtido && order.Status != OrderStatus.Pausa)
                throw new InvalidOperationException("La orden no está en un estado válido para surtir.");

            // Cambiar a "En surtido" si no lo estaba
            if (order.Status != OrderStatus.EnSurtido)
            {
                order.StartPicking();
                await _historyRepository.AddAsync(new OrderStatusHistoryEntity(order.Id, order.Status, dto.UserId, "Inicio del proceso de surtido."));
            }

            var product = await _productRepository.GetByCodeAsync(dto.Barcode);
            if (product == null) throw new InvalidOperationException("Producto no encontrado por código de barras.");

            var detail = order.Details.FirstOrDefault(d => d.ProductId == product.Id);
            if (detail == null) throw new InvalidOperationException("El producto escaneado no pertenece a esta orden.");

            detail.Scan(dto.Quantity);

            // Verificar si todo está surtido para auto-cerrar
            bool allPicked = order.Details.All(d => d.QuantityOrdered == d.QuantityScanned);
            if (allPicked)
            {
                order.Complete();
                
                // Registro de finalización en el historial
                var completeHistory = new OrderStatusHistoryEntity(
                    order.Id, 
                    order.Status, 
                    dto.UserId, 
                    "Surtido finalizado al 100%. Orden cerrada automáticamente.");
                await _historyRepository.AddAsync(completeHistory);

                // Notificación al vendedor
                Console.WriteLine($"[NOTIFICACIÓN VENDEDOR] La orden {order.Id} (Folio: {order.Folio}) ha sido FINALIZADA. El cliente ya puede recoger su pedido.");
            }

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _historyRepository.SaveChangesAsync();

            return order.ToDto();
        }

        public async Task<OrderResponseDto> PauseOrderAsync(Guid orderId, Guid userId)
        {
            var order = await _orderRepository.GetWithDetailsAsync(orderId);
            if (order == null) throw new InvalidOperationException("Orden no encontrada.");

            order.Pause();

            // Registro de pausa en el historial
            var pauseHistory = new OrderStatusHistoryEntity(order.Id, order.Status, userId, "Surtido pausado por el Jefe de Bodega.");
            await _historyRepository.AddAsync(pauseHistory);

            // Notificación al vendedor
            Console.WriteLine($"[NOTIFICACIÓN VENDEDOR] La orden {order.Id} (Folio: {order.Folio}) ha sido PAUSADA. Por favor, revisa con el cliente posibles faltantes o demoras.");

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            await _historyRepository.SaveChangesAsync();

            return order.ToDto();
        }
    }
}

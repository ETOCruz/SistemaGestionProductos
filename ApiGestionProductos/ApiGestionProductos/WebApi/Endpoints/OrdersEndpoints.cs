using Application.DTOs.Orders;
using Application.UseCase.Orders;
using Application.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WebApi.Endpoints
{
    public static class OrdersEndpoints
    {
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/orders")
                .WithTags("Orders");

            // Crear Cotización
            group.MapPost("/", async (CreateOrderDto dto, CreateOrderUseCase useCase) =>
            {
                try
                {
                    var order = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/v1/orders/{order.Id}", order);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("CreateOrder")
            .WithSummary("Crea una nueva cotización de orden.")
            .Produces<OrderResponseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Autorizar Orden (Valida stock y descuenta)
            group.MapPost("/{id:guid}/authorize", async (Guid id, Guid userId, AuthorizeOrderUseCase useCase) =>
            {
                try
                {
                    var result = await useCase.ExecuteAsync(id, userId);
                    if (!result.Success)
                        return Results.UnprocessableEntity(result);

                    return Results.Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AuthorizeOrder")
            .WithSummary("Autoriza la orden validando y descontando inventario.")
            .Produces<AuthorizationResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError);


            // Escanear Producto (Surtido)
            group.MapPut("/{id:guid}/scan", async (Guid id, ScanProductDto dto, UpdateSurtidoUseCase useCase) =>
            {
                try
                {
                    var order = await useCase.ExecuteAsync(id, dto);
                    return Results.Ok(order);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("ScanProduct")
            .WithSummary("Registra el escaneo de un producto para el surtido.")
            .Produces<OrderResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Pausar Orden
            group.MapPut("/{id:guid}/pause", async (Guid id, Guid userId, UpdateSurtidoUseCase useCase) =>
            {
                try
                {
                    var order = await useCase.PauseOrderAsync(id, userId);
                    return Results.Ok(order);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("PauseOrder")
            .WithSummary("Cambia el estado de la orden a Pausa.")
            .Produces<OrderResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            // Obtener Detalle de Orden
            group.MapGet("/{id:guid}", async (Guid id, GetOrderDetailsUseCase useCase) =>
            {
                try
                {
                    var order = await useCase.ExecuteAsync(id);
                    return Results.Ok(order);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("GetOrderById")
            .WithSummary("Obtiene los detalles y estatus de una orden.")
            .Produces<OrderResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            // Obtener Todas las Ordenes con Paginación y Filtro
            group.MapGet("/", async (OrderStatus? status, int? pageNumber, int? pageSize, GetAllOrdersUseCase useCase) =>
            {
                var orders = await useCase.ExecuteAsync(status, pageNumber ?? 1, pageSize ?? 10);
                return Results.Ok(orders);
            })
            .WithName("GetAllOrders")
            .WithSummary("Lista todas las órdenes registradas con paginación y filtro por estatus.")
            .Produces<PagedResponseDto<OrderResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            // Validar Stock antes de Crear Orden
            group.MapPost("/validate-stock", async (ValidateStockRequestDto dto, ValidateStockUseCase useCase) =>
            {
                try
                {
                    var result = await useCase.ExecuteAsync(dto);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("ValidateStock")
            .WithSummary("Valida la disponibilidad de stock para una lista de productos.")
            .Produces<ValidateStockResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}

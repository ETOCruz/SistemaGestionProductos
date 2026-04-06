using Application.UseCase.Products;
using Application.DTOs.Producs;

namespace WebApi.Endpoints
{
    public static class ProductsEndpoints
    {

        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/products")
                .WithTags("Products");

            group.MapPost("/", async (CreateProductDto dto, CreateProductUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/api/person/{product.Id}", product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            })
            .WithName("CreateProduct")
            .WithSummary("Crea un nuevo Producto")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id}", async (Guid id, GetProductByIdUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(id);
                    return Results.Ok(product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("GetProductById")
            .WithSummary("Obtener una product por su id")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async (GetAllProductsUseCase useCase) =>
            {
                try
                {
                    var Products = await useCase.ExecuteAsync();
                    return Results.Ok(Products);
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            })
            .WithName("GetAllProducts")
            .WithSummary("Obtener todos los productos registrados")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateProductDto dto, UpdateProductUseCase useCase) =>
            {
                if (id != dto.Id)
                {
                    return Results.BadRequest("Los ids no corresponden");
                }
                try
                {
                    var product = await useCase.ExecuteAsync(dto);
                    return Results.Ok(product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            })
            .WithName("UpdatePerson")
            .WithSummary("Actualiza la información de una producto existente")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, DeleteProductUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ex.Message);
                }
            })
            .WithName("DeleteProduct")
            .WithSummary("Elimina una producto registrado")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/barcode/{barcode}", async (string barcode, GetProductByBarcodeUseCase useCase) =>
            {
                try
                {
                    var product = await useCase.ExecuteAsync(barcode);
                    return Results.Ok(product);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { error = ex.Message });
                }
            })
            .WithName("GetProductByBarcode")
            .WithSummary("Obtener un producto por su barcode")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}

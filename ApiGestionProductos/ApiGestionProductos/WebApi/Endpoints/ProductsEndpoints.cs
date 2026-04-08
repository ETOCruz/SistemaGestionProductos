using Application.UseCase.Products;
using Application.UseCase.Categories;
using Application.DTOs.Producs;
using Application.DTOs;
using Tools;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using System;

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
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/", async (int? pageNumber, int? pageSize, GetAllProductsUseCase useCase) =>
            {
                try
                {
                    var products = await useCase.ExecuteAsync(pageNumber ?? 1, pageSize ?? 10);
                    return Results.Ok(products);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetAllProducts")
            .WithSummary("Obtener todos los productos registrados con paginación")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/search", async (string? name, int? categoryId, int? subCategoryId, int? pageNumber, int? pageSize, SearchProductsUseCase useCase) =>
            {
                try
                {
                    var result = await useCase.ExecuteAsync(name, categoryId, subCategoryId, pageNumber ?? 1, pageSize ?? 10);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("SearchProducts")
            .WithSummary("Buscar productos por nombre, categoría o subcategoría con paginación")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

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
            })
            .WithName("UpdateProduct")
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
            })
            .WithName("DeleteProduct")
            .WithSummary("Elimina una producto registrado")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}/add-stock", async (Guid id, AddStockDto dto, AddProductStockUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id, dto);
                    return Results.Ok(new { message = "Stock incrementado exitosamente." });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddProductStock")
            .WithSummary("Agrega stock (entradas) al inventario actual de un producto registrado")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
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
            })
            .WithName("GetProductByBarcode")
            .WithSummary("Obtener un producto por su barcode")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/categories", async (GetCategoriesUseCase useCase) =>
            {
                var result = await useCase.ExecuteAsync();
                return Results.Ok(result);
            })
            .WithName("GetCategories")
            .WithSummary("Obtener todos los categorías con sus subcategorías");

            group.MapGet("/categories/{id:int}/subcategories", async (int id, GetSubCategoriesUseCase useCase) =>
            {
                var result = await useCase.ExecuteAsync(id);
                return Results.Ok(result);
            })
            .WithName("GetSubCategories")
            .WithSummary("Obtener subcategorías por el id de la categoría");
        }
    }
}

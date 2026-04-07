using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Application.UseCase.Products;
using Application.UseCase.Orders;
using Application.UseCase.Categories;
using WebApi.Endpoints;
using Data;
using Domain;
using Domain.Abstractions;
using Data.Repository;
using WebApi.Middleware;
using System.IO;
using Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/*
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
*/

#region Repositories
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Hace falta la conexión a la base de datos");

builder.Services.AddData(connectionString);

// builder.Services.AddScoped<IRepository<ProductEntity, Guid>, ProductRepository>();
#endregion

#region ProductUseCase
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<GetAllProductsUseCase>();
builder.Services.AddScoped<GetProductByBarcodeUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<SearchProductsUseCase>();
#endregion

#region CategoryUseCase
builder.Services.AddScoped<GetCategoriesUseCase>();
builder.Services.AddScoped<GetSubCategoriesUseCase>();
#endregion

#region OrderUseCase
builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<AuthorizeOrderUseCase>();
builder.Services.AddScoped<UpdateSurtidoUseCase>();
builder.Services.AddScoped<GetOrderDetailsUseCase>();
builder.Services.AddScoped<GetAllOrdersUseCase>();
builder.Services.AddScoped<ValidateStockUseCase>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// configure logs directory and middleware
var logsDir = Path.Combine(app.Environment.ContentRootPath, "logs");
Directory.CreateDirectory(logsDir);
var logPath = Path.Combine(logsDir, "app.log");
app.UseMiddleware<ExceptionLoggingMiddleware>(logPath);

#region Endpoints
app.MapProductsEndpoints();
app.MapOrdersEndpoints();
#endregion

app.Run();

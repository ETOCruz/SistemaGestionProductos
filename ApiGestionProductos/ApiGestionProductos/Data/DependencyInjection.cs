using Data.Persistence;
using Data.Repository;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services,
            string connectionString)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRepository<ProductEntity, Guid>, ProductRepository>();
            services.AddScoped<ICodeRepository<ProductEntity>, ProductRepository>();
            services.AddScoped<IRepository<WarehouseEntity, Guid>, WarehouseRepository>();
            services.AddScoped<IRepository<CategoryEntity, int>, CategoryRepository>();
            services.AddScoped<IRepository<SubCategoryEntity, int>, SubCategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();

            return services;
        }
    }
}

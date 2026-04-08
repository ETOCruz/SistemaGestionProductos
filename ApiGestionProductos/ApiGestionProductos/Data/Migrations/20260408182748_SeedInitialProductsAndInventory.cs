using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialProductsAndInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Barcode", "Name", "Price", "ProductDescription", "SubCategoryId" },
                values: new object[,]
                {
                    { new Guid("20624587-9688-4842-801e-5a98219c3fb6"), "198866", "Tarja eb de 1 tinas", 25.50m, "Modelo Eb onen33229-1012hf", 4 },
                    { new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"), "198867", "Tarja eb de 3 tinas", 25.50m, "Modelo Eb onen33229-1012ht", 4 },
                    { new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"), "1234563", "Xiaomi POCO F7 Ultra", 25.50m, "Fruta seleccionada de alta calidad", 2 },
                    { new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"), "123456789", "Asus Vivobook", 25.50m, "The ASUS Vivobook is a versatile", 2 },
                    { new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"), "198865", "Tarja eb de 2 tinas", 25.50m, "Modelo Eb onen33229-1012hd", 4 },
                    { new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"), "163062", "Escritorio de baal con 3 cajones color nogal", 25.50m, "Modelo Baal walnut b", 3 }
                });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "ProductId", "WarehouseId", "StockQuantity" },
                values: new object[,]
                {
                    { new Guid("20624587-9688-4842-801e-5a98219c3fb6"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("20624587-9688-4842-801e-5a98219c3fb6"), new Guid("22222222-2222-2222-2222-222222222222"), 50 },
                    { new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"), new Guid("22222222-2222-2222-2222-222222222222"), 50 },
                    { new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"), new Guid("22222222-2222-2222-2222-222222222222"), 50 },
                    { new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"), new Guid("22222222-2222-2222-2222-222222222222"), 50 },
                    { new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"), new Guid("22222222-2222-2222-2222-222222222222"), 50 },
                    { new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"), new Guid("11111111-1111-1111-1111-111111111111"), 100 },
                    { new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"), new Guid("22222222-2222-2222-2222-222222222222"), 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("20624587-9688-4842-801e-5a98219c3fb6"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("20624587-9688-4842-801e-5a98219c3fb6"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumns: new[] { "ProductId", "WarehouseId" },
                keyValues: new object[] { new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20624587-9688-4842-801e-5a98219c3fb6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("39a0bd15-b635-422a-b1bf-54dc66cb1581"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("3fef1b0e-1fbf-4f77-ae32-e5828cdd7fec"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("46f3f79d-0488-4f61-abcd-517cce578cd4"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1779efe-cdb4-4e67-9dd0-5da791841132"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d0151aab-c32e-4b50-b629-e380f50342c8"));
        }
    }
}

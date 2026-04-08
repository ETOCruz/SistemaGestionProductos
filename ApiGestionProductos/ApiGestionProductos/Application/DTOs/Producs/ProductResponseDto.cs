using System;

namespace Application.DTOs.Products
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public static ProductResponseDto FromEntity(Domain.ProductEntity p)
        {
            if (p == null) return null;
            return new ProductResponseDto
            {
                Id = p.Id,
                Barcode = p.Barcode,
                Name = p.Name,
                Description = p.ProductDescription,
                Price = p.Price,
                SubCategoryId = p.SubCategoryId,
                SubCategoryName = p.SubCategory?.Name ?? "N/A",
                CategoryId = p.SubCategory?.Category?.Id,
                CategoryName = p.SubCategory?.Category?.Name ?? "N/A"
            };
        }
    }
}

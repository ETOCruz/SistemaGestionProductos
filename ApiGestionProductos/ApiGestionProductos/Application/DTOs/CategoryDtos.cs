using System;

namespace Application.DTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<SubCategoryResponseDto> SubCategories { get; set; } = new List<SubCategoryResponseDto>();
    }

    public class SubCategoryResponseDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

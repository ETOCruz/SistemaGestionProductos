using Application.DTOs;
using Domain;
using Domain.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Categories
{
    public class GetCategoriesUseCase
    {
        private readonly IRepository<CategoryEntity, int> _categoryRepo;

        public GetCategoriesUseCase(IRepository<CategoryEntity, int> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<CategoryResponseDto>> ExecuteAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                SubCategories = c.SubCategories.Select(s => new SubCategoryResponseDto
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    Name = s.Name
                }).ToList()
            });
        }
    }
}

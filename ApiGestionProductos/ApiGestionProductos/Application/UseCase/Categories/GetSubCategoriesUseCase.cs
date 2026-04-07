using Application.DTOs;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UseCase.Categories
{
    public class GetSubCategoriesUseCase
    {
        private readonly IRepository<SubCategoryEntity, int> _subRepo;

        public GetSubCategoriesUseCase(IRepository<SubCategoryEntity, int> subRepo)
        {
            _subRepo = subRepo;
        }

        public async Task<IEnumerable<SubCategoryResponseDto>> ExecuteAsync(int categoryId)
        {
            var all = await _subRepo.GetAllAsync();
            return all.Where(s => s.CategoryId == categoryId)
                .Select(s => new SubCategoryResponseDto
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    Name = s.Name
                });
        }
    }
}

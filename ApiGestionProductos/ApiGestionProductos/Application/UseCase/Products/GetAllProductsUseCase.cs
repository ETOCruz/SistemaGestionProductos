using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCase.Products
{
    public class GetAllProductsUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public GetAllProductsUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProductEntity>> ExecuteAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}

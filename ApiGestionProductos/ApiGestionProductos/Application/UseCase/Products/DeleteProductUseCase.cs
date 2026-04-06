using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCase.Products
{
    public class DeleteProductUseCase
    {
        private readonly IRepository<ProductEntity, Guid> _repository;

        public DeleteProductUseCase(IRepository<ProductEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                throw new InvalidOperationException($"No se encontró una producto con el ID: {id}");
            }

            await _repository.DeleteAsync(product);

            await _repository.SaveChangesAsync();
        }
    }
}

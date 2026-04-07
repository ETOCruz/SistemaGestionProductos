using System;
using System.Collections.Generic;

namespace Domain
{
    public class SubCategoryEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public int CategoryId { get; private set; }

        public CategoryEntity Category { get; private set; } = null!;
        public ICollection<ProductEntity> Products { get; private set; } = new List<ProductEntity>();

        public SubCategoryEntity(string name, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la subcategoría no puede estar vacío.", nameof(name));

            Name = name.Trim();
            CategoryId = categoryId;
        }

        // For EF Core
        private SubCategoryEntity() { }
    }
}

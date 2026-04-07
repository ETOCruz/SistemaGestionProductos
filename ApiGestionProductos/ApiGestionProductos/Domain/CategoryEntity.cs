using System;
using System.Collections.Generic;

namespace Domain
{
    public class CategoryEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public ICollection<SubCategoryEntity> SubCategories { get; private set; } = new List<SubCategoryEntity>();

        public CategoryEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(name));

            Name = name.Trim();
        }

        // For EF Core
        private CategoryEntity() { }
    }
}
